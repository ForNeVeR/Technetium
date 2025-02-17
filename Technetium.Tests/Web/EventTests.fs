module Technetium.Tests.Web.EventTests

open System
open System.Threading.Tasks

open NodaTime
open Technetium.Web.Requests
open Xunit

open Technetium.TestFramework.WebServer

let private getEvents client =
    GetObject<List<EventDto>>(client, "/api/event")

let private testDateTime = LocalDateTime(2020, 1, 1, 12, 00).InZoneStrictly DateTimeZoneProviders.Tzdb["Asia/Barnaul"]
let private testDateTimeSerialized = "2020-01-01T12:00:00+07 Asia/Barnaul"

[<Fact>]
let ``Server returns an empty event list by default``(): Task = WithDefaultWebClient(fun client -> task {
    let! result = getEvents client
    Assert.Empty result
})

[<Fact>]
let ``Server returns a newly-created empty``(): Task = WithDefaultWebClient(fun client -> task {
    let event = EventDto(1, testDateTime, TimeSpan.FromMinutes 30.0, "Event Title", null)

    let! emptyList = getEvents client
    Assert.Empty emptyList

    do! PutObject(client, "/api/event", event)
    let! result = getEvents client
    Assert.Equivalent(event, Assert.Single result)
})

[<Fact>]
let ``Event ids are auto-generated on server``(): Task = WithDefaultWebClient(fun client -> task {
    let event = EventDto(777, testDateTime, TimeSpan.FromMinutes 30.0, "Event Title", null)
    for _ in 1..3 do
        do! PutObject(client, "/api/event", event)
    let! result = getEvents client
    Assert.Collection(
        result,
        Action<_>(fun x -> Assert.Equal(1, x.Id.Value)),
        Action<_>(fun x -> Assert.Equal(2, x.Id.Value)),
        Action<_>(fun x -> Assert.Equal(3, x.Id.Value))
    )
})

[<Fact>]
let ``Server accepts a well-formed JSON``(): Task = WithDefaultWebClient(fun client -> task {
    let jsonData =
         $"""{{
    "StartDateTime": "{testDateTimeSerialized}",
    "Duration": "00:30:00",
    "Title": "Event Title",
    "Description": null
}}"""
    let event = EventDto(1, testDateTime, TimeSpan.FromMinutes 30.0, "Event Title", null)

    do! PutJson(client, "/api/event", jsonData)
    let! result = getEvents client
    Assert.Equivalent(event, Assert.Single result)
})
