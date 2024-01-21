module Technetium.Console.ConsoleUtil

open System
open System.Threading
open System.Threading.Tasks

open IcedTasks.CancellableTasks

let WithConsoleCancellationToken(cancellableTask: CancellableTask<'a>): Task<'a> = task {
    use cts = new CancellationTokenSource()
    use _ = Console.CancelKeyPress.Subscribe(fun args ->
        printfn "Cancellation requested."
        args.Cancel <- true // tell the runtime that we'll handle the cancellation
        cts.Cancel()
    )
    return! cancellableTask cts.Token
}
