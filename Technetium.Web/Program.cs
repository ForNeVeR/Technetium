using Microsoft.EntityFrameworkCore;
using Technetium.Data;
using Technetium.Web.Extensions;

namespace Technetium.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services
            .AddDbContext<TechnetiumDataContext>(opts => opts
                .UseSqlite(builder.Configuration.GetConnectionString(MainConfiguration.DatabaseConnectionStringName)))
            .AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.ConfigureTechnetiumJson());

        await using var application = builder.Build();
        await application.ApplyMigrationsAsync();

        application.MapGet("/", () => Results.LocalRedirect("/index.html"));
        application.UseStaticFiles();
        application.MapControllers();

        await application.RunAsync();
    }
}
