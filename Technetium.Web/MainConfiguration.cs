using System.Text.Json;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace Technetium.Web;

public static class MainConfiguration
{
    public const string DatabaseConnectionStringName = "Database";

    public static JsonSerializerOptions ConfigureTechnetiumJson(this JsonSerializerOptions options) =>
        options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
}
