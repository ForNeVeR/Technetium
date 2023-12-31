module Technetium.Tests.Core.ScheduleTests

open System

open Xunit

open Technetium.Core.Schedule

let private baseTime = DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero)

type private TestTask =
    {
        Name: string
        Duration: TimeSpan
    }
    interface ITask with
        member this.Duration = this.Duration

let private createTask name duration = {
    Name = name
    Duration = duration
}

[<Fact>]
let ``Plan for simple schedule``(): unit =
    let schedule = {
        SpansToSchedule = [| struct(baseTime, baseTime + TimeSpan.FromHours 8.0) |]
    }
    let task1 = createTask "task1" <| TimeSpan.FromHours 1.0
    let task2 = createTask "task2" <| TimeSpan.FromHours 1.0
    let task3 = createTask "task3" <| TimeSpan.FromHours 1.0
    let plan = PreparePlan schedule [| task1; task2; task3 |]
    Assert.Equal(
        [|
            struct(baseTime, task1)
            struct(baseTime + TimeSpan.FromHours 1.0, task2)
            struct(baseTime + TimeSpan.FromHours 2.0, task3)
        |],
        plan.Scheduled
    )
    Assert.Empty plan.Unscheduled

[<Fact>]
let ``Plan with unscheduled tasks``(): unit =
    let schedule = {
        SpansToSchedule = [| struct(baseTime, baseTime + TimeSpan.FromHours 1.0) |]
    }
    let task1 = createTask "task1" <| TimeSpan.FromHours 1.0
    let task2 = createTask "task2" <| TimeSpan.FromHours 1.0
    let plan = PreparePlan schedule [| task1; task2 |]
    Assert.Equal(
        [|
            struct(baseTime, task1)
        |],
        plan.Scheduled
    )
    Assert.Equal([| task2 |], plan.Unscheduled)

[<Fact>]
let ``Plan with task spanning across two periods``(): unit =
    let schedule = {
        SpansToSchedule = [|
            struct(baseTime, baseTime + TimeSpan.FromHours 1.0)
            struct(baseTime + TimeSpan.FromHours 2.0, baseTime + TimeSpan.FromHours 3.0)
        |]
    }
    let task1 = createTask "task1" <| TimeSpan.FromHours 1.5
    let task2 = createTask "task2" <| TimeSpan.FromHours 0.5
    let plan = PreparePlan schedule [| task1; task2 |]
    Assert.Equal(
        [|
            struct(baseTime, task1)
            struct(baseTime + TimeSpan.FromHours 2.5, task2)
        |],
        plan.Scheduled
    )
    Assert.Empty plan.Unscheduled

[<Fact>]
let ``Plan with task too big to fit into one period``(): unit =
    let schedule = {
        SpansToSchedule = [|
            struct(baseTime, baseTime + TimeSpan.FromHours 1.0)
            struct(baseTime + TimeSpan.FromHours 2.0, baseTime + TimeSpan.FromHours 2.5)
            struct(baseTime + TimeSpan.FromHours 3.0, baseTime + TimeSpan.FromHours 4.0)
        |]
    }
    let task1 = createTask "task1" <| TimeSpan.FromHours 2.0
    let task2 = createTask "task2" <| TimeSpan.FromHours 0.5
    let plan = PreparePlan schedule [| task1; task2 |]
    Assert.Equal(
        [|
            struct(baseTime, task1)
            struct(baseTime + TimeSpan.FromHours 3.5, task2)
        |],
        plan.Scheduled
    )
    Assert.Empty plan.Unscheduled
