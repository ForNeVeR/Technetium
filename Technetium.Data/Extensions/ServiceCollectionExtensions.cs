using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Technetium.Data.Interfaces;

namespace Technetium.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddScoped<IMigrationRunner, MigrationRunner>()
            .AddDbContext<TechnetiumDataContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("Database")));
}
