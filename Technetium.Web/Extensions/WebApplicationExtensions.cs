using Microsoft.EntityFrameworkCore;
using Technetium.Data;

namespace Technetium.Web.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Migrating database");
        }

        var databaseContext = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>();
        await databaseContext.Database.MigrateAsync();

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Database migration done");
        }
    }
}
