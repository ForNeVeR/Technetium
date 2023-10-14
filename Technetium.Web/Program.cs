using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Technetium.Data.Extensions;
using Technetium.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDatabase(configuration);
builder.Environment.WebRootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "wwwroot");
builder.Environment.WebRootFileProvider = new PhysicalFileProvider(builder.Environment.WebRootPath);

var application = builder.Build();

await application.ApplyMigrationsAsync();

application.UseStaticFiles();
application.MapGet("/", () => Results.LocalRedirect("/index.html"));

application.Run();
