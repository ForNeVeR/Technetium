module Technetium.Google.GoogleTasks

open System
open System.Collections.Generic
open System.Globalization
open System.Threading.Tasks

open Google.Apis.Tasks.v1
open Google.Apis.Tasks.v1.Data

open Technetium.Core.Schedule

type TaskCommand =
    | ScheduleToTime of Task * DateTimeOffset

[<Literal>]
let EntitiesPerRequest = 5 // TODO: Update to 100 later; 5 is only for multi-page load tests

type TaskService(oAuthToken: string) =
    let googleService = new TasksService()

    let executeChainedLoad request itemsExtractor nextPageTokenExtractor: Task<IReadOnlyList<_>> = task {
        let result = ResizeArray()
        let mutable nextPageToken = null
        let mutable proceed = true
        while proceed do
            let! response = request nextPageToken
            result.AddRange <| itemsExtractor response

            match nextPageTokenExtractor response with
            | null ->
                proceed <- false
            | token ->
                nextPageToken <- token

        return result
    }

    interface IDisposable with
        member _.Dispose() =
            googleService.Dispose()

    member _.GetAllTaskLists(): Task<IReadOnlyList<TaskList>> =
        executeChainedLoad (
            fun pageToken ->
                let request = googleService.Tasklists.List()
                request.OauthToken <- oAuthToken
                request.MaxResults <- EntitiesPerRequest
                request.PageToken <- pageToken
                request.ExecuteAsync()
            ) (_.Items) (_.NextPageToken)

    member _.GetAllTasks(
        taskListId: string,
        periodStart: DateTimeOffset,
        periodEnd: DateTimeOffset
    ): Task<IReadOnlyList<Task>> = task {

        let loadTasks(pageToken: string) =
            let request = googleService.Tasks.List taskListId
            request.OauthToken <- oAuthToken
            request.DueMin <- periodStart.ToString("s", CultureInfo.InvariantCulture)
            request.DueMax <- periodEnd.ToString("s", CultureInfo.InvariantCulture)
            request.MaxResults <- 100
            request.PageToken <- pageToken
            request.ExecuteAsync()

        let result = ResizeArray()
        let mutable nextPageToken = null
        let mutable proceed = true
        while proceed do
            let! tasks = loadTasks nextPageToken
            result.AddRange tasks.Items

            match tasks.NextPageToken with
            | null ->
                proceed <- false
            | token ->
                nextPageToken <- token

        return result
    }

    member _.Apply(taskListId: string, commands: TaskCommand seq): System.Threading.Tasks.Task = task {
        for ScheduleToTime(task, time) in commands do
            let patch = Task(Due = time.ToString("s", CultureInfo.InvariantCulture))
            let request = googleService.Tasks.Patch(patch, taskListId, task.Id)
            let! _ = request.ExecuteAsync()
            ()
    }

type GoogleTask(task: Task) =
    member _.OriginalTask = task
    interface ITask with
        member _.Duration = TimeSpan.FromHours 1.0

let WrapTasks(tasks: Task seq): GoogleTask seq =
    tasks |> Seq.map GoogleTask

type CruftBehavior = {
    MoveToOffset: DateTimeOffset
    AssumedDuration: TimeSpan
}

let UpdateCommands (plan: SchedulePlan<GoogleTask>) (cruftBehavior: CruftBehavior): TaskCommand seq =
    let maybeMove (task: Task) (time: DateTimeOffset) =
        if task.Due = time.ToString("s", CultureInfo.InvariantCulture) then
            None
        else
            Some(ScheduleToTime(task, time))

    let processScheduled() =
        plan.Scheduled
        |> Seq.choose(fun struct (time, gt) -> maybeMove gt.OriginalTask time)

    let processUnscheduled() = seq {
        let mutable currentTime = cruftBehavior.MoveToOffset
        for task in plan.Unscheduled do
            yield ScheduleToTime(task.OriginalTask, currentTime)
            currentTime <- currentTime + cruftBehavior.AssumedDuration
    }

    seq {
        yield! processScheduled()
        yield! processUnscheduled()
    }
