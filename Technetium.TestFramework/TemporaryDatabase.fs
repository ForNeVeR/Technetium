module Technetium.TestFramework.TemporaryDatabase

open System.IO
open System.Threading.Tasks

open Microsoft.Data.Sqlite

let WithDatabaseFilePath(action: string -> Task): Task = task {
    let dbFilePath = Path.GetTempFileName()
    try
        do! action dbFilePath
    finally
        SqliteConnection.ClearAllPools()
        File.Delete dbFilePath
}
