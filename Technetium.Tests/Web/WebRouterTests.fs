module Technetium.Tests.Web.WebRouterTests

open System.Net
open System.Net.Http.Headers
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc.Testing

open Xunit

let private withWebApplication action = task {
    use app = new WebApplicationFactory<Technetium.Web.Program>()
    do! action app
}

let private withWebClient options action = withWebApplication(fun app -> task {
    use client = app.CreateClient options
    do! action client
})

let private withDefaultWebClient action = withWebClient (WebApplicationFactoryClientOptions()) action

[<Fact>]
let ``Root route should resolve to the index page``(): Task =
    let options = WebApplicationFactoryClientOptions(AllowAutoRedirect = false)
    withWebClient options (fun client -> task {
        let! response = client.GetAsync "/"
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode)
    })

[<Fact>]
let ``Index page should contain HTML``(): Task = withDefaultWebClient(fun client -> task {
    let! response = client.GetAsync "/index.html"
    Assert.Equal(HttpStatusCode.OK, response.StatusCode)
    Assert.Equal(MediaTypeHeaderValue("text/html"), response.Content.Headers.ContentType)
})

[<Fact>]
let ``404 page should work properly``(): Task = withDefaultWebClient(fun client -> task {
    let! response = client.GetAsync "/blah"
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode)
})
