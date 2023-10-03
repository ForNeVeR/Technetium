namespace Technetium.Data.Interfaces;

public interface IMigrationRunner : IDisposable
{
    Task MigrateAsync();
}
