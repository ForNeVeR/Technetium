using Microsoft.EntityFrameworkCore;
using Technetium.Data;
using Technetium.Web.Extensions;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddDbContext<TechnetiumDataContext>(opts => opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

var application = builder.Build();
await application.ApplyMigrationsAsync();

application.MapGet("/", () => Results.LocalRedirect("/index.html"));
application.UseStaticFiles();

application.Run();
