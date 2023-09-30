using Microsoft.EntityFrameworkCore;
using Technetium.Data;
using Technetium.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<TechnetiumDataContext>(opts => opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();
await app.MigrateDatabase();

app.MapGet("/", () => "Hello World!");

app.Run();
