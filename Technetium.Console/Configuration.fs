namespace Technetium.Console

open System
open System.IO
open System.Text.Json
open System.Threading.Tasks

open Technetium.Google.GoogleTasks
open Technetium.Core.Schedule

type Configuration =
    {
        PeriodStart: DateTimeOffset
        PeriodEnd: DateTimeOffset
        Schedule: Schedule
        CruftBehavior: CruftBehavior
    }
    static member Read(input: Stream): Task<Configuration> = task {
        return! JsonSerializer.DeserializeAsync input
    }
