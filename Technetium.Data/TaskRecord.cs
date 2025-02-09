using NodaTime;

namespace Technetium.Data;

public record TaskRecord(
    long Id,
    string ExternalId,
    LocalDateTime? ScheduledTime,
    string Name,
    string Description,
    long Order
);
