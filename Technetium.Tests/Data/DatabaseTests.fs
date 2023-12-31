module Technetium.Tests.Data.DatabaseTests

open System.Threading.Tasks
open Technetium.Data
open Microsoft.EntityFrameworkCore
open Xunit

[<Fact>]
let ``Database initializes properly``(): Task = task {
    let factory = TechnetiumDataDesignFactory()
    use context = factory.CreateDbContext Array.empty
    do! context.Database.MigrateAsync()
}
