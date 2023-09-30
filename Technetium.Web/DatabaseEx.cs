using Technetium.Data;

namespace Technetium.Web;

internal static class DatabaseEx
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Migrating database.");

        var context = scope.ServiceProvider.GetRequiredService<TechnetiumDataContext>();
        await context.Migrate();

        logger.LogInformation("Database migration done.");
    }
}
