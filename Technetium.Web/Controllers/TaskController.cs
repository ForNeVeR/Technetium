using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task Import([FromBody] IReadOnlyList<GoogleTaskList> input)
    {
        var index = 0L;
        var tasksToImport = input.SelectMany(x => x.Tasks).ToList();
        var importedTaskIds = tasksToImport.Select(MapId);

        var existingTasks = await db.TaskRecords
            .Where(t => importedTaskIds.Contains(t.ExternalId))
            .ToDictionaryAsync(x => x.ExternalId!);

        foreach (var task in tasksToImport)
        {
            var taskRecord = Convert(task, index++);
            if  (existingTasks.TryGetValue(taskRecord.ExternalId!, out var existingTask))
                Update(task, existingTask);
            else
                db.TaskRecords.Add(taskRecord);
        }

        await db.SaveChangesAsync();
    }

    private static string MapId(GoogleTask task) => $"google:{task.Id ?? throw new BadHttpRequestException("No task id provided")}";
    private static TaskRecord Convert(GoogleTask input, long order) => new() {
        Id = 0L,
        ExternalId = MapId(input),
        ScheduledTime = input.Due == null
            ? null
            : InstantPattern.ExtendedIso.Parse(input.Due).GetValueOrThrow().InUtc().LocalDateTime,
        Title = input.Title ?? "",
        Description = input.Notes ?? "",
        Order = order
    };

    private static void Update(GoogleTask source, TaskRecord destination)
    {
        var mapResult = Convert(source, 0L);
        // TODO: Decide what to do with orders of tasks on such update
        destination.ScheduledTime = mapResult.ScheduledTime;
        destination.Title = mapResult.Title;
        destination.Description = mapResult.Description;
        destination.Order = mapResult.Order;
    }
}
