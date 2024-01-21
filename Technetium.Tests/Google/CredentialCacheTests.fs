module Technetium.Tests.Google.CredentialCacheTests

open System.Threading.Tasks

open Google.Apis.Util.Store
open Xunit

open Technetium.Google

[<Fact>]
let ``Stored data could be read afterwards``(): Task = task {
    use cache = new InMemoryCredentialCache()
    let store = cache :> IDataStore

    let! noData = store.GetAsync "empty"
    Assert.Null noData

    do! store.StoreAsync("test", "check")
    let! data = store.GetAsync "test"
    Assert.Equal("check", data)
}
