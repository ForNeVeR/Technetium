module Technetium.Tests.Web.EventTests

open System.Threading.Tasks
open Technetium.Web.Controllers
open Xunit

open Technetium.TestFramework.WebServer

[<Fact>]
let ``Server returns an empty event list by default``(): Task = WithDefaultWebClient(fun client -> task {
    let! result = GetObject<List<EventDto>>(client, "/api/event")
    Assert.Empty result
})

[<Fact>]
let ``Server returns a newly-created empty``(): unit = Assert.True false
