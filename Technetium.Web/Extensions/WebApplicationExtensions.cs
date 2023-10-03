using Technetium.Data.Interfaces;

namespace Technetium.Web.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Shortcut for <see cref="IMigrationRunner.MigrateAsync"/>
    /// </summary>
    /// <param name="application">Instance of <see cref="WebApplication"/></param>
    public static Task UseMigrationRunnerAsync(this WebApplication application)
        => application
            .Services
            .GetRequiredService<IMigrationRunner>()
            .MigrateAsync();
}
