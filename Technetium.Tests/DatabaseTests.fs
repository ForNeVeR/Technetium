module DatabaseTests

open System.Threading.Tasks
open Technetium.Data
open Technetium.Data.Interfaces
open Microsoft.EntityFrameworkCore
open Xunit

type FakeMigrationRunner(databaseContext: TechnetiumDataContext) =
    member this.MigrateAsync() = (this :> IMigrationRunner).MigrateAsync()
    interface IMigrationRunner with
        member this.MigrateAsync() = databaseContext.Database.MigrateAsync()
        member this.Dispose() = databaseContext.Dispose()

[<Fact>]
let ``Database initializes properly``(): Task = task {
    let factory = TechnetiumDataDesignFactory()
    use context = factory.CreateDbContext Array.empty
    use migrationRunner = new FakeMigrationRunner(context)
    do! migrationRunner.MigrateAsync()
}
