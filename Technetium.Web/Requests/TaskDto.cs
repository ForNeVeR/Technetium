using NodaTime;
using Technetium.Data;

namespace Technetium.Web.Requests;

public record TaskDto(
    long Id,
    string? ExternalId,
    string Title,
    ZonedDateTime? ScheduledAt)
{
    public static TaskDto FromDb(TaskRecord record) => new(
        record.Id,
        record.ExternalId,
        record.Title,
        record.ScheduledAt
    );
}
