using Task = Google.Apis.Tasks.v1.Data.Task;

namespace Technetium.Web.Requests;

public record GoogleTaskList(IReadOnlyList<Task> Tasks);
