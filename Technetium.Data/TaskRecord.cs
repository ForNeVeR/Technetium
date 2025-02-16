using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Technetium.Data;

[Index(nameof(ExternalId), IsUnique = true)]
public record TaskRecord
{
    public long Id { get; set; }
    public string? ExternalId { get; set; }
    public LocalDateTime? ScheduledTime { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public long Order { get; set; }
}
