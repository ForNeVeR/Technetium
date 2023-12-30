open Technetium.Console.GoogleAuth

open Google.Apis.Tasks.v1

let private printTasks (user: string) (clientSecretFilePath: string) = task {
    let! clientSecret = readClientSecretFile clientSecretFilePath
    let! accessToken = getAuthenticationToken user clientSecret
    use service = new TasksService()
    let request = service.Tasklists.List()
    request.OauthToken <- accessToken.AccessToken
    let taskLists = request.Execute()

    for item in taskLists.Items do
        printfn $"{item.Id}: {item.Title}"
        let request = service.Tasks.List(item.Id)
        request.OauthToken <- accessToken.AccessToken
        let tasks = request.Execute()
        for task in tasks.Items do
            printfn $"    {task.Title}"

    return ()
}

[<EntryPoint>]
let main: string[] -> int = function
    | [| user; clientSecretFilePath |] ->
        (printTasks user clientSecretFilePath).GetAwaiter().GetResult()
        0
    | _ ->
        printfn "Usage: Technetium.Console <clientSecretFilePath>"
        1
