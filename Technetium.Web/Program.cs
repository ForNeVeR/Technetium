using Technetium.Data.Extensions;
using Technetium.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDatabase(configuration);

var application = builder.Build();

await application.ApplyMigrationsAsync();

application.MapGet("/", () => "Hello World!");

application.Run();
