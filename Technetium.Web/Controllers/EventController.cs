using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technetium.Data;
using Technetium.Web.Requests;

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
