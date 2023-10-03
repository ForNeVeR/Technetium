using Microsoft.EntityFrameworkCore;

namespace Technetium.Data;

public class TechnetiumDataContext : DbContext
{
    public TechnetiumDataContext(DbContextOptions options) : base(options) {}

    public Task MigrateAsync() => Database.MigrateAsync();
}
