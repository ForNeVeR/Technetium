module Technetium.Tests.Data.DatabaseTests

open System.Threading.Tasks

open Microsoft.EntityFrameworkCore
open Xunit

open Technetium.Data
open Technetium.TestFramework

[<Fact>]
let ``Database initializes properly``(): Task = TemporaryDatabase.WithDatabaseFilePath(fun path -> task {
    let options = DbContextOptionsBuilder().UseSqlite($"Data Source={path}").Options
    use context = new TechnetiumDataContext(options)
    do! context.Database.MigrateAsync()
})
