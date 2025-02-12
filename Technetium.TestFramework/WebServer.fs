module Technetium.TestFramework.WebServer

open System.Net.Http
open System.Net.Http.Headers
open System.Net.Mime
open System.Text.Json
open System.Threading.Tasks

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.Configuration

open Technetium.Web

let WithWebApplication(action: WebApplicationFactory<_> -> Task): Task =
    TemporaryDatabase.WithDatabaseFilePath(fun path -> task {
        use app =
            (new WebApplicationFactory<Program>())
                .WithWebHostBuilder(
                    fun (builder: IWebHostBuilder) ->
                        builder.ConfigureAppConfiguration(fun context ->
                            context.AddInMemoryCollection(Map.ofArray[|
                                $"ConnectionStrings:{MainConfiguration.DatabaseConnectionStringName}",
                                $"Data Source={path}"
                            |]) |> ignore
                        ) |> ignore
                )
        do! action app
    })

let WithWebClient options action = WithWebApplication(fun app -> task {
    use client = app.CreateClient options
    do! action client
})

let WithDefaultWebClient action = WithWebClient (WebApplicationFactoryClientOptions()) action

let private assertSuccessAndGetContent(response: HttpResponseMessage) = task {
    let! content = response.Content.ReadAsStringAsync()
    if not response.IsSuccessStatusCode then
        failwithf $"HTTP error code {response.StatusCode}: {content}"
    return content
}

let JsonOptions = JsonSerializerOptions(JsonSerializerDefaults.Web).ConfigureTechnetiumJson()

let GetObject<'a>(client: HttpClient, url: string): Task<'a> = task {
    let! response = client.GetAsync(url)
    let! content = assertSuccessAndGetContent response
    return JsonSerializer.Deserialize(content, JsonOptions)
}

let PutObject(client: HttpClient, url: string, object: 'a): Task = task {
    use content = new StringContent(
        JsonSerializer.Serialize(object, options = JsonOptions),
        MediaTypeHeaderValue(MediaTypeNames.Application.Json)
    )
    let! response = client.PutAsync(url, content)
    let! _ = assertSuccessAndGetContent response
    ()
}

let private DoJson action (client: HttpClient) (url: string) json = task {
    use content = new StringContent(json, MediaTypeHeaderValue(MediaTypeNames.Application.Json))
    let! response = action client (url, content)
    let! _ = assertSuccessAndGetContent response
    ()
}

let PutJson(client: HttpClient, url: string, json: string): Task =
    DoJson _.PutAsync client url json

let PostJson(client: HttpClient, url: string, json: string): Task =
    DoJson _.PostAsync client url json
