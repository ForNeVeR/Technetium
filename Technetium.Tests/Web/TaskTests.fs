module Technetium.Tests.Web.TaskTests

open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.DependencyInjection
open Microsoft.EntityFrameworkCore
open NodaTime
open Technetium.Data
open Technetium.TestFramework.WebServer
open Xunit

[<Fact>]
let ``Import should create tasks in the database``(): Task =
    WithWebApplication(fun app -> task {
        use client = app.CreateClient(WebApplicationFactoryClientOptions())

        let tasks = """
{
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
}
"""
        do! PostJson(client, "/api/task/import/google", tasks)

        use scope = app.Services.CreateScope()
        use db = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>()
        let! tasks = db.TaskRecords.ToListAsync()
        Assert.Equivalent([|
            TaskRecord(
                1L,
                "google:WEx3RnNyQlJvTmdaa3FPeg",
                LocalDateTime(2024, 1, 5, 0, 0),
                "Language",
                "",
                0L
            )
            TaskRecord(
                2L,
                "google:N2ExSzNuS0NSalNZbUdhag",
                LocalDateTime(2024, 1, 12, 0, 0),
                "Garbage",
                "",
                1L
            )
        |], tasks)
    })
