using NodaTime;
using Technetium.Data;

namespace Technetium.Web.Requests;

public record EventDto(int? Id, ZonedDateTime StartDateTime, TimeSpan Duration, string Title, string? Description)
{
    internal static EventDto FromDb(Event @event)
    {
        var tz = DateTimeZoneProviders.Tzdb[@event.StartTimeZone];
        return new(
            @event.Id,
            @event.StartDateTime.InZoneStrictly(tz),
            @event.Duration,
            @event.Title,
            @event.Description);
    }

    internal static Event ToDb(EventDto dto) => new(
        dto.Id ?? 0,
        dto.StartDateTime.LocalDateTime,
        dto.StartDateTime.Zone.Id,
        dto.Duration,
        dto.Title,
        dto.Description
    );
}
