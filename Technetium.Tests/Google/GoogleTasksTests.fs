module Technetium.Tests.Google.GoogleTasksTests

open System
open System.Globalization

open Google.Apis.Tasks.v1.Data
open Xunit

open Technetium.Google.GoogleTasks
open Technetium.Core.Schedule


let private baseTime = DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero)
let private cruftBehavior = {
    MoveToOffset = baseTime.AddDays 1.0
    AssumedDuration = TimeSpan.FromHours(0.5)
}

let private createTask title (due: DateTimeOffset) =
    Task(Title = title, Due = due.ToString("s", CultureInfo.InvariantCulture))

let wrap task = GoogleTask task

[<Fact>]
let ``Update for scheduled tasks``(): unit =
    let task1 = createTask "task1" baseTime
    let task2 = createTask "task2" (baseTime + TimeSpan.FromHours(1.0))
    let task3 = createTask "task3" (baseTime - TimeSpan.FromHours(1.0))
    let plan = {
        Scheduled = [|
            struct(baseTime - TimeSpan.FromHours(1.0), wrap task1)
            struct(baseTime + TimeSpan.FromHours(1.0), wrap task2)
            struct(baseTime, wrap task3)
        |]
        Unscheduled = Array.empty
    }
    let commands = UpdateCommands plan cruftBehavior
    Assert.Equal<TaskCommand>([|
        ScheduleToTime(task1, baseTime - TimeSpan.FromHours(1.0))
        ScheduleToTime(task3, baseTime)
    |], commands)

[<Fact>]
let ``Diff for unscheduled tasks``(): unit =
    let task1 = createTask "task1" baseTime
    let task2 = createTask "task2" baseTime
    let task3 = createTask "task3" baseTime
    let plan = {
        Scheduled = Array.empty
        Unscheduled = [|
            wrap task1
            wrap task2
            wrap task3
        |]
    }
    let commands = UpdateCommands plan cruftBehavior
    Assert.Equal<TaskCommand>([|
        ScheduleToTime(task1, baseTime.AddDays 1.0)
        ScheduleToTime(task2, baseTime.AddDays 1.0 + TimeSpan.FromHours(0.5))
        ScheduleToTime(task3, baseTime.AddDays 1.0 + TimeSpan.FromHours(1.0))
    |], commands)
