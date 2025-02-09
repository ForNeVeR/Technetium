using Microsoft.EntityFrameworkCore;

namespace Technetium.Data;

public class TechnetiumDataContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<TaskRecord> TaskRecords { get; set; }

    public TechnetiumDataContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(builder => builder.UseNodaTime());
    }
}
