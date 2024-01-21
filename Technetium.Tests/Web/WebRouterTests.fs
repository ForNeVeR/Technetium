module Technetium.Tests.Web.WebRouterTests

open System.Net
open System.Net.Http.Headers
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc.Testing
open Xunit

open Technetium.TestFramework.WebServer

[<Fact>]
let ``Root route should resolve to the index page``(): Task =
    let options = WebApplicationFactoryClientOptions(AllowAutoRedirect = false)
    WithWebClient options (fun client -> task {
        let! response = client.GetAsync "/"
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode)
    })

[<Fact>]
let ``Index page should contain HTML``(): Task = WithDefaultWebClient(fun client -> task {
    let! response = client.GetAsync "/index.html"
    Assert.Equal(HttpStatusCode.OK, response.StatusCode)
    Assert.Equal(MediaTypeHeaderValue("text/html"), response.Content.Headers.ContentType)
})

[<Fact>]
let ``404 page should work properly``(): Task = WithDefaultWebClient(fun client -> task {
    let! response = client.GetAsync "/blah"
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode)
})
