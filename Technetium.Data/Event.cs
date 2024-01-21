using NodaTime;

namespace Technetium.Data;

public record Event(
    int Id,
    LocalDateTime StartDateTime,
    string StartTimeZone,
    TimeSpan Duration,
    string Title,
    string? Description
);
