module Technetium.TestFramework.WebServer

open System.IO
open System.Net.Http
open System.Net.Http.Headers
open System.Net.Mime
open System.Text.Json
open System.Threading.Tasks

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.Configuration

open Technetium.Web

let private WithWebApplication action = task {
    let tempDbFile = Path.GetTempFileName()
    try
        use app =
            (new WebApplicationFactory<Program>())
                .WithWebHostBuilder(
                    fun (builder: IWebHostBuilder) ->
                        builder.ConfigureAppConfiguration(fun context ->
                            context.AddInMemoryCollection(Map.ofArray[|
                                $"ConnectionStrings:{MainConfiguration.DatabaseConnectionStringName}",
                                $"Data Source={tempDbFile}"
                            |]) |> ignore
                        ) |> ignore
                )
        do! action app
    finally
        // TODO: Cannot delete because it's being used by something
        // File.Delete tempDbFile
        ()
}

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
    return JsonSerializer.Deserialize content
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

let PutJson(client: HttpClient, url: string, json: string): Task = task {
    use content = new StringContent(json, MediaTypeHeaderValue(MediaTypeNames.Application.Json))
    let! response = client.PutAsync(url, content)
    let! _ = assertSuccessAndGetContent response
    ()
}
