open System
open System.Globalization
open System.IO
open System.Threading.Tasks

open Technetium.Console
open Technetium.Core
open Technetium.Google.GoogleAuth
open Technetium.Google.GoogleTasks

type private AuthInformation = {
    UserName: string
    ClientSecretFilePath: string
}

let private printPlan commands =
    let print color (text: string) =
        Console.ForegroundColor <- color
        Console.Write text
    let println() = Console.WriteLine()

    let oldColor = Console.ForegroundColor
    try
        for ScheduleToTime(task, offset) in commands do
            print oldColor "Move from "
            print ConsoleColor.Yellow task.Due
            print oldColor " to "
            print ConsoleColor.Yellow <| offset.ToString("s", CultureInfo.InvariantCulture)
            print oldColor " task "
            print ConsoleColor.Yellow task.Id
            print oldColor ": "
            print ConsoleColor.Green task.Title
            println()
    finally
        Console.ForegroundColor <- oldColor

type private UpdateMode =
    | PrintPlan
    | ApplyPlan

let private processTasks (taskService: TaskService) taskListId (config: Configuration) mode : Task<unit> = task {
    let! tasks = taskService.GetAllTasks(taskListId, config.PeriodStart, config.PeriodEnd)
    let plan = Schedule.PreparePlan config.Schedule (WrapTasks tasks)
    let commands = UpdateCommands plan config.CruftBehavior
    match mode with
    | PrintPlan -> printPlan commands
    | ApplyPlan -> do! taskService.Apply(taskListId, commands)
}

let private authenticate (authInfo: AuthInformation) = task {
    let! clientSecret = readClientSecretFile authInfo.ClientSecretFilePath
    return! getAuthenticationToken authInfo.UserName clientSecret
}

let private getTaskList(taskService: TaskService) = task {
    let! taskLists = taskService.GetAllTaskLists()
    return taskLists |> Seq.exactlyOne
}

let private asyncMain user clientSecretFilePath configFilePath: Task<int> = task {
    let authInfo = { UserName = user; ClientSecretFilePath = clientSecretFilePath }
    use configFile = File.OpenRead configFilePath
    let! config = Configuration.Read configFile

    let! accessCredentials = authenticate authInfo
    use taskService = new TaskService(accessCredentials.AccessToken)

    let! taskList = getTaskList taskService

    do! processTasks taskService taskList.Id config PrintPlan
    return 0
}

[<EntryPoint>]
let main: string[] -> int = function
    | [| user; clientSecretFilePath; pathToConfigFile |] ->
        (asyncMain user clientSecretFilePath pathToConfigFile).GetAwaiter().GetResult()
    | _ ->
        printfn "Usage: Technetium.Console <googleUserName> <clientSecretFilePath> <configFilePath>"
        1
