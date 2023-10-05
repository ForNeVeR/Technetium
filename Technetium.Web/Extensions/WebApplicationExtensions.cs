using Microsoft.EntityFrameworkCore;
using Technetium.Data;

namespace Technetium.Web.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Migrating database");

        var databaseContext = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>();
        await databaseContext.Database.MigrateAsync();

        logger.LogInformation("Database migration done");
    }
}
