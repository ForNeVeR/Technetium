using Microsoft.AspNetCore.Mvc;
using NodaTime.Text;
using Technetium.Data;
using Technetium.Web.Requests;
using Task = System.Threading.Tasks.Task;
using GoogleTask = Google.Apis.Tasks.v1.Data.Task;

namespace Technetium.Web.Controllers;

[Route("/api/task")]
public class TaskController(TechnetiumDataContext db) : Controller
{
    [HttpPost("import/google")]
    public Task Import([FromBody] IReadOnlyList<GoogleTaskList> input)
    {
        var index = 0L;
        foreach (var task in input.SelectMany(x => x.Tasks))
        {
            var taskRecord = Convert(task, index++);
            db.TaskRecords.Add(taskRecord);
        }

        return db.SaveChangesAsync();
    }

    private static TaskRecord Convert(GoogleTask input, long order) => new(
        Id: 0L,
        ExternalId: $"google:{input.Id}",
        ScheduledTime: input.Due == null
            ? null
            : InstantPattern.ExtendedIso.Parse(input.Due).GetValueOrThrow().InUtc().LocalDateTime,
        Name: input.Title ?? "",
        Description: input.Notes ?? "",
        Order: order
    );
}
