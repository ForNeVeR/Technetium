module Technetium.Google.GoogleAuth

open System.IO
open System.Text.Json
open System.Threading
open System.Threading.Tasks
open Google.Apis.Auth.OAuth2
open Google.Apis.Auth.OAuth2.Responses

[<Literal>]
let private TasksScope = "https://www.googleapis.com/auth/tasks"

let readClientSecretFile(path: string): Task<ClientSecrets> = task {
    use content = File.OpenRead path
    let! json = JsonDocument.ParseAsync(content)
    let installed = json.RootElement.GetProperty("installed")
    return ClientSecrets(
        ClientId = installed.GetProperty("client_id").GetString(),
        ClientSecret = installed.GetProperty("client_secret").GetString()
    )
}

let getAuthenticationToken (user: string) (secret: ClientSecrets): Task<TokenResponse> = task {
    let! result = GoogleWebAuthorizationBroker.AuthorizeAsync(
        secret,
        [| TasksScope |],
        user,
        CancellationToken.None
    )
    return result.Token
}
