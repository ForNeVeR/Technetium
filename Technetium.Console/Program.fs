﻿open System.IO

open IcedTasks

open Technetium.Console
open Technetium.Console.ConsoleUtil
open Technetium.Google
open Technetium.Google.GoogleAuth

type private AuthInformation = {
    UserName: string
    ClientSecretFilePath: string
}

let private authenticate (authInfo: AuthInformation) = cancellableTask {
    use cache = new InMemoryCredentialCache()
    let! clientSecret = readClientSecretFile authInfo.ClientSecretFilePath
    return! getAuthenticationToken cache authInfo.UserName clientSecret
}

let private asyncMain user clientSecretFilePath configFilePath = cancellableTask {
    let authInfo = { UserName = user; ClientSecretFilePath = clientSecretFilePath }
    use configFile = File.OpenRead configFilePath
    let! _config = Configuration.Read configFile

    let! _accessCredentials = authenticate authInfo
    return 0
}

[<EntryPoint>]
let main: string[] -> int = function
    | [| user; clientSecretFilePath; pathToConfigFile |] ->
        let task = WithConsoleCancellationToken(asyncMain user clientSecretFilePath pathToConfigFile)
        task.GetAwaiter().GetResult()
    | args ->
        printfn "Usage: Technetium.Console <userName> <clientSecretFilePath> <configFilePath>"
        if Array.isEmpty args then 0 else 1
