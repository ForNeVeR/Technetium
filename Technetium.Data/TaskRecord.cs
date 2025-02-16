using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Technetium.Data;

[Index(nameof(ExternalId), IsUnique = true)]
public record TaskRecord
{
    public long Id { get; set; }
    public string? ExternalId { get; set; }
    public Instant? ScheduledAtInstant { get; set; }

    /// <summary>If null then UTC.</summary>
    public string? ScheduledAtTimeZoneId { get; set; }

    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public long Order { get; set; }

    [NotMapped]
    public ZonedDateTime? ScheduledAt
    {
        get
        {
            if (ScheduledAtInstant is not { } scheduledAtInstant) return null;
            var timeZone = ScheduledAtTimeZoneId == null
                ? DateTimeZone.Utc
                : DateTimeZoneProviders.Tzdb.GetZoneOrNull(ScheduledAtTimeZoneId) ?? DateTimeZone.Utc;
            return new ZonedDateTime(scheduledAtInstant, timeZone);
        }
        set
        {
            if (value is { } dateTime)
            {
                ScheduledAtInstant = dateTime.ToInstant();
                ScheduledAtTimeZoneId = dateTime.Zone.Id;
            }
            else
            {
                ScheduledAtInstant = null;
                ScheduledAtTimeZoneId = null;
            }
        }
    }
}
