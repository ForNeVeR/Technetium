open System.IO
open System.Threading.Tasks

open Technetium.Console
open Technetium.Google.GoogleAuth

type private AuthInformation = {
    UserName: string
    ClientSecretFilePath: string
}

let private authenticate (authInfo: AuthInformation) = task {
    let! clientSecret = readClientSecretFile authInfo.ClientSecretFilePath
    return! getAuthenticationToken authInfo.UserName clientSecret
}

let private asyncMain user clientSecretFilePath configFilePath: Task<int> = task {
    let authInfo = { UserName = user; ClientSecretFilePath = clientSecretFilePath }
    use configFile = File.OpenRead configFilePath
    let! _config = Configuration.Read configFile

    let! _accessCredentials = authenticate authInfo
    return 0
}

[<EntryPoint>]
let main: string[] -> int = function
    | [| user; clientSecretFilePath; pathToConfigFile |] ->
        (asyncMain user clientSecretFilePath pathToConfigFile).GetAwaiter().GetResult()
    | args ->
        printfn "Usage: Technetium.Console <userName> <clientSecretFilePath> <configFilePath>"
        if Array.isEmpty args then 0 else 1

