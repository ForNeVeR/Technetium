module Technetium.Google.GoogleAuth

open System.IO
open System.Text.Json

open Google.Apis.Auth.OAuth2
open Google.Apis.Auth.OAuth2.Responses
open Google.Apis.Util.Store
open IcedTasks

[<Literal>]
let private TasksScope = "https://www.googleapis.com/auth/tasks"

let readClientSecretFile(path: string): CancellableTask<ClientSecrets> = cancellableTask {
    use content = File.OpenRead path
    let! ct = CancellableTask.getCancellationToken()
    let! json = JsonDocument.ParseAsync(content, cancellationToken = ct)
    let installed = json.RootElement.GetProperty("installed")
    return ClientSecrets(
        ClientId = installed.GetProperty("client_id").GetString(),
        ClientSecret = installed.GetProperty("client_secret").GetString()
    )
}

let getAuthenticationToken (store: IDataStore) (user: string) (secret: ClientSecrets): CancellableTask<TokenResponse> = cancellableTask {
    let! ct = CancellableTask.getCancellationToken()
    let! result = GoogleWebAuthorizationBroker.AuthorizeAsync(
        secret,
        [| TasksScope |],
        user,
        ct,
        dataStore = store
    )
    return result.Token
}
