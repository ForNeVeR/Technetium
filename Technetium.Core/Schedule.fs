module Technetium.Core.Schedule

open System
open System.Collections.Generic

type DateTimeSpan = (struct (DateTimeOffset * DateTimeOffset))

type Schedule = {
    SpansToSchedule: DateTimeSpan seq
}

type ITask =
    abstract member Duration: TimeSpan

type SchedulePlan<'Task when 'Task :> ITask> = {
    Scheduled: IReadOnlyList<struct (DateTimeOffset * 'Task)>
    Unscheduled: IReadOnlyList<'Task>
}

let PreparePlan<'Task when 'Task :> ITask> (schedule: Schedule) (tasks: 'Task seq): SchedulePlan<'Task> =
    let periodEnumerator = schedule.SpansToSchedule.GetEnumerator()

    let scheduled = ResizeArray()
    let unscheduled = ResizeArray()

    let mutable currentPeriod = None
    let mutable currentFreeStartTime = None

    let findFreeStartTime() =
        if Option.isNone currentFreeStartTime && periodEnumerator.MoveNext() then
            currentPeriod <- Some periodEnumerator.Current
            let struct (start, _) = periodEnumerator.Current
            currentFreeStartTime <- Some start
        currentFreeStartTime

    let rec moveCurrentTime startTime duration =
        let endTime = startTime + duration
        let struct (_, periodEnd) = Option.get currentPeriod
        if endTime < periodEnd then
            currentFreeStartTime <- Some endTime
        elif endTime = periodEnd then
            currentFreeStartTime <- None
            currentPeriod <- None
        else // endTime > periodEnd
            let remainder = endTime - periodEnd
            currentFreeStartTime <- None
            currentPeriod <- None
            match findFreeStartTime() with
            | None -> () // no more periods left, give up
            | Some newPeriodStartTime ->
                moveCurrentTime newPeriodStartTime remainder

    for task in tasks do
        match findFreeStartTime() with
        | Some startTime ->
            scheduled.Add(struct (startTime, task))
            moveCurrentTime startTime task.Duration
        | None -> unscheduled.Add task

    {
        Scheduled = scheduled
        Unscheduled = unscheduled
    }
