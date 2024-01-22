module Technetium.Tests.Web.JsonTests

open System.Text.Json
open NodaTime
open Technetium.TestFramework
open Xunit

[<Fact>]
let ``JSON date time should be deserialized correctly``(): unit =
    let dateTime = LocalDateTime(2020, 1, 15, 23, 31, 0).InZoneStrictly(DateTimeZoneProviders.Tzdb["Asia/Barnaul"])
    let json = JsonSerializer.Serialize(dateTime, WebServer.JsonOptions)
    Assert.Equal(dateTime, JsonSerializer.Deserialize<ZonedDateTime>(json, WebServer.JsonOptions))

    let json = "\"2020-01-15T23:31:00+07 Asia/Barnaul\""
    let value = JsonSerializer.Deserialize<ZonedDateTime>(json, WebServer.JsonOptions)
    Assert.Equal(dateTime, value)
