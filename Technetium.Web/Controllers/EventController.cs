using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Technetium.Data;

namespace Technetium.Web.Controllers;

[Route("/api/event")]
public class EventController(TechnetiumDataContext db) : Controller
{
    [HttpPut]
    public async Task<IActionResult> CreateEvent([FromBody] EventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var @event = EventDto.ToDb(dto) with { Id = 0 };
        db.Events.Add(@event);
        await db.SaveChangesAsync();

        return Created();
    }

    [HttpGet]
    public async Task<IEnumerable<EventDto>> GetEvents()
    {
        var events = await db.Events.ToListAsync();
        return events.Select(EventDto.FromDb);
    }
}

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
