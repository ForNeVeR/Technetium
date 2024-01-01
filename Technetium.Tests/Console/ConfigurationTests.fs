module Technetium.Tests.Console.ConfigurationTests

open System
open System.IO
open System.Text
open System.Threading.Tasks

open Technetium.Console
open Xunit

[<Fact>]
let ``Configuration is read correctly``(): Task = task {
    let input = """
                {
                    "periodStart": "2023-12-31T00:00:00Z",
                    "periodEnd": "2024-01-01T00:00:00Z",
                    "schedule": {
                        "spansToSchedule": [
                            ["2023-12-31T00:00:00Z", "2024-12-31T02:00:00Z"],
                            ["2023-12-31T03:00:00Z", "2024-12-31T06:00:00Z"]
                        ]
                    },
                    "cruftBehavior": {
                        "moveToOffset": "2024-01-01T00:00:00Z",
                        "assumedDuration": "00:30:00"
                    }
                }
                """
    use stream = new MemoryStream(Encoding.UTF8.GetBytes input)
    let! config = Configuration.Read stream
    Assert.Equal(
    {
        PeriodStart = DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero)
        PeriodEnd = DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)
        Schedule = {
            SpansToSchedule = [|
                struct (DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero),
                        DateTimeOffset(2024, 12, 31, 2, 0, 0, TimeSpan.Zero))
                struct (DateTimeOffset(2023, 12, 31, 3, 0, 0, TimeSpan.Zero),
                        DateTimeOffset(2024, 12, 31, 6, 0, 0, TimeSpan.Zero))
            |]
        }
        CruftBehavior = {
            MoveToOffset = DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)
            AssumedDuration = TimeSpan.FromHours 0.5
        }
    }, config)
}
