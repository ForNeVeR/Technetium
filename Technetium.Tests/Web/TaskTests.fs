module Technetium.Tests.Web.TaskTests

open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.DependencyInjection
open Microsoft.EntityFrameworkCore
open NodaTime
open Technetium.Data
open Technetium.TestFramework.WebServer
open Xunit

let private TestTasks tasks action =
    WithWebApplication(fun app -> task {
        use client = app.CreateClient(WebApplicationFactoryClientOptions())
        do! PostJson(client, "/api/task/import/google", tasks)
        do! action app
    })

[<Fact>]
let ``Import should create tasks in the database``(): Task =
    let tasks = """
[{
    "TaskList": {
        "ETag": "\u0022LTYxNTQ0NzY4OA\u0022",
        "Id": "MTI0MDY4MTA4NjgxMzg1NzMxNjM6MDow",
        "Kind": "tasks#taskList",
        "SelfLink": "https://www.googleapis.com/tasks/v1/users/@me/lists/MTI0MDY4MTA4NjgxMzg1NzMxNjM6MDow",
        "Title": "My Tasks",
        "Updated": "2024-01-05T20:29:28.661Z"
    },
    "Tasks": [
        {
            "Completed": "2024-01-05T20:29:27.000Z",
            "Deleted": null,
            "Due": "2024-01-05T00:00:00.000Z",
            "ETag": "\u0022LTYxNTQ0NzcxOQ\u0022",
            "Hidden": true,
            "Id": "WEx3RnNyQlJvTmdaa3FPeg",
            "Kind": "tasks#task",
            "Links": [],
            "Notes": null,
            "Parent": null,
            "Position": "09999998295513432445",
            "SelfLink": "https://www.googleapis.com/tasks/v1/lists/MTI0MDY4MTA4NjgxMzg1NzMxNjM6MDow/tasks/WEx3RnNyQlJvTmdaa3FPeg",
            "Status": "completed",
            "Title": "Language",
            "Updated": "2024-01-05T20:29:28.000Z"
        },
        {
            "Completed": null,
            "Deleted": null,
            "Due": "2024-01-12T00:00:00.000Z",
            "ETag": "\u0022LTYxNjE4NjQ0Mw\u0022",
            "Hidden": null,
            "Id": "N2ExSzNuS0NSalNZbUdhag",
            "Kind": "tasks#task",
            "Links": [],
            "Notes": null,
            "Parent": null,
            "Position": "00000000000000000000",
            "SelfLink": "https://www.googleapis.com/tasks/v1/lists/MTI0MDY4MTA4NjgxMzg1NzMxNjM6MDow/tasks/N2ExSzNuS0NSalNZbUdhag",
            "Status": "needsAction",
            "Title": "Garbage",
            "Updated": "2024-01-05T20:17:09.000Z"
        }
    ]
}]
"""
    TestTasks tasks (fun app -> task {
        use scope = app.Services.CreateScope()
        use db = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>()
        let! tasks = db.TaskRecords.ToListAsync()
        Assert.Equivalent([|
            TaskRecord(
                Id = 1L,
                ExternalId = "google:WEx3RnNyQlJvTmdaa3FPeg",
                ScheduledTime = LocalDateTime(2024, 1, 5, 0, 0),
                Title = "Language",
                Description = "",
                Order = 0L
            )
            TaskRecord(
                Id = 2L,
                ExternalId = "google:N2ExSzNuS0NSalNZbUdhag",
                ScheduledTime = LocalDateTime(2024, 1, 12, 0, 0),
                Title = "Garbage",
                Description = "",
                Order = 1L
            )
        |], tasks)
    })

[<Fact>]
let ``Import should merge similar tasks``(): Task =
    let tasks1 = """
[{
    "Tasks": [
        {
            "Completed": "2024-01-05T20:29:27.000Z",
            "Deleted": null,
            "Due": "2024-01-05T00:00:00.000Z",
            "ETag": "\u0022LTYxNTQ0NzcxOQ\u0022",
            "Hidden": true,
            "Id": "WEx3RnNyQlJvTmdaa3FPeg",
            "Kind": "tasks#task",
            "Links": [],
            "Notes": null,
            "Parent": null,
            "Position": "09999998295513432445",
            "SelfLink": "https://www.googleapis.com/tasks/v1/lists/MTI0MDY4MTA4NjgxMzg1NzMxNjM6MDow/tasks/WEx3RnNyQlJvTmdaa3FPeg",
            "Status": "completed",
            "Title": "Language",
            "Updated": "2024-01-05T20:29:28.000Z"
        }
    ]
}]
"""

    let tasks2 = """
[{
    "Tasks": [
        {
            "Completed": "2024-01-05T20:29:27.000Z",
            "Deleted": null,
            "Due": "2024-01-06T00:00:00.000Z",
            "ETag": "\u0022LTYxNTQ0NzcxOQ\u0022",
            "Hidden": true,
            "Id": "WEx3RnNyQlJvTmdaa3FPeg",
            "Kind": "tasks#task",
            "Links": [],
            "Notes": null,
            "Parent": null,
            "Position": "09999998295513432445",
            "SelfLink": "https://www.googleapis.com/tasks/v1/lists/MTI0MDY4MTA4NjgxMzg1NzMxNjM6MDow/tasks/WEx3RnNyQlJvTmdaa3FPeg",
            "Status": "completed",
            "Title": "Language",
            "Updated": "2024-01-05T20:29:28.000Z"
        }
    ]
}]
"""

    TestTasks tasks1 (fun app -> task {
        do! task {
            use scope = app.Services.CreateScope()
            use db = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>()
            let! tasks = db.TaskRecords.ToListAsync()
            Assert.Equivalent([|
                TaskRecord(
                    Id = 1L,
                    ExternalId = "google:WEx3RnNyQlJvTmdaa3FPeg",
                    ScheduledTime = LocalDateTime(2024, 1, 5, 0, 0),
                    Title = "Language",
                    Description = "",
                    Order = 0L
                )
            |], tasks, strict = true)
        }

        use client = app.CreateClient(WebApplicationFactoryClientOptions())
        do! PostJson(client, "/api/task/import/google", tasks2)
        use scope = app.Services.CreateScope()
        use db = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>()
        let! tasks = db.TaskRecords.ToListAsync()
        Assert.Equivalent([|
            TaskRecord(
                Id = 1L,
                ExternalId = "google:WEx3RnNyQlJvTmdaa3FPeg",
                ScheduledTime = LocalDateTime(2024, 1, 6, 0, 0),
                Title = "Language",
                Description = "",
                Order = 0L
            )
        |], tasks, strict = true)
    })
