using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Technetium.Data;

/// <remarks>This is used by the EF migration infrastructure only.</remarks>
[UsedImplicitly]
public class TechnetiumDataDesignFactory : IDesignTimeDbContextFactory<TechnetiumDataContext>
{
    public TechnetiumDataContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=:memory:")
            .Options;
        return new TechnetiumDataContext(options);
    }
}
