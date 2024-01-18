namespace Technetium.Console

open System
open System.IO
open System.Text.Json
open System.Text.Json.Serialization
open System.Text.Json.Serialization.Metadata
open System.Threading.Tasks

open Technetium.Core.Schedule

[<CLIMutable>] // for deserialization
type ScheduleConfiguration =
    {
        SpansToSchedule: DateTimeOffset[][]
    }
    member this.AsSchedule(): Schedule =
        {
            SpansToSchedule = this.SpansToSchedule |> Array.map(fun tuple ->
                match tuple with
                | [| span1; span2 |] -> struct(span1, span2)
                | _ -> failwithf $"Invalid pair of timespans in SpansToSchedule: %A{tuple}"
            )
        }

[<CLIMutable>] // for deserialization
type Configuration =
    {
        PeriodStart: DateTimeOffset
        PeriodEnd: DateTimeOffset
        Schedule: ScheduleConfiguration
    }

    static member private JsonOptions = JsonSerializerOptions(
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        TypeInfoResolver = DefaultJsonTypeInfoResolver().WithAddedModifier(fun typeInfo ->
            typeInfo.Properties |> Seq.iter(fun p -> p.IsRequired <- true)
        )
    )

    static member Read(input: Stream): Task<Configuration> = task {
        return! JsonSerializer.DeserializeAsync(input, Configuration.JsonOptions)
    }
