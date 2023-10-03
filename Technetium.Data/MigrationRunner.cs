using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Technetium.Data.Interfaces;

namespace Technetium.Data;

internal sealed class MigrationRunner : IMigrationRunner
{
    private readonly ILogger<MigrationRunner> _logger;
    
    private readonly TechnetiumDataContext _databaseContext;

    public MigrationRunner(
        ILogger<MigrationRunner> logger,
        TechnetiumDataContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task MigrateAsync()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Migrating database");
        }

        await _databaseContext.Database.MigrateAsync();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Database migration done");
        }
    }

    /// <inheritdoc />
    public void Dispose()
        => _databaseContext.Dispose();
}
