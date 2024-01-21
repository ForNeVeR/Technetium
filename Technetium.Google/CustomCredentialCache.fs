namespace Technetium.Google

open System
open System.Collections.Generic
open System.Threading
open Google.Apis.Util.Store

type InMemoryCredentialCache() =

    let data = Dictionary<string, obj>()
    let lock = new SemaphoreSlim(1)

    let withLock action = task {
        do! lock.WaitAsync()
        try
            return action()
        finally
            lock.Release() |> ignore
    }

    interface IDataStore with
        member this.ClearAsync() = withLock data.Clear
        member this.DeleteAsync key = withLock(fun () -> data.Remove key)
        member this.GetAsync key = withLock(fun () ->
            match data.TryGetValue key with
            | true, value -> value :?> _
            | false, _ -> Unchecked.defaultof<_>
        )
        member this.StoreAsync(key, value) = withLock(fun () ->
            data[key] <- value
        )

    interface IDisposable with
        member this.Dispose() = lock.Dispose()
