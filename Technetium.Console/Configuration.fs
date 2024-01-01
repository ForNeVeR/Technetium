namespace Technetium.Console

open System
open System.IO
open System.Runtime.CompilerServices
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading.Tasks

open Technetium.Core.Schedule
open Technetium.Google.GoogleTasks

#nowarn "202" // TODO[#13]: Better JSON management

[<CLIMutable; RequiredMember>]
type ScheduleConfiguration =
    {
        [<RequiredMember>] SpansToSchedule: DateTimeOffset[][]
    }
    member this.AsSchedule(): Schedule =
        {
            SpansToSchedule = this.SpansToSchedule |> Array.map(fun tuple ->
                match tuple with
                | [| span1; span2 |] -> struct(span1, span2)
                | _ -> failwithf $"Invalid pair of timespans in SpansToSchedule: %A{tuple}"
            )
        }

[<CLIMutable; RequiredMember>]
type Configuration =
    {
        [<RequiredMember>] PeriodStart: DateTimeOffset
        [<RequiredMember>] PeriodEnd: DateTimeOffset
        [<RequiredMember>] Schedule: ScheduleConfiguration
        [<RequiredMember>] CruftBehavior: CruftBehavior
    }
    static member Read(input: Stream): Task<Configuration> = task {
        let opts = JsonSerializerOptions(
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
        )
        return! JsonSerializer.DeserializeAsync(input, opts)
    }
