module Technetium.TestFramework.WebServer

open System.Net.Http
open System.Text.Json
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc.Testing

let private WithWebApplication action = task {
    use app = new WebApplicationFactory<Technetium.Web.Program>()
    do! action app
}

let WithWebClient options action = WithWebApplication(fun app -> task {
    use client = app.CreateClient options
    do! action client
})

let WithDefaultWebClient action = WithWebClient (WebApplicationFactoryClientOptions()) action

let GetObject<'a>(client: HttpClient, url: string): Task<'a> = task {
    let! response = client.GetAsync(url)
    let! content = response.Content.ReadAsStringAsync()
    if not response.IsSuccessStatusCode then
        failwithf $"HTTP error code {response.StatusCode}: {content}"
    return JsonSerializer.Deserialize content
}
