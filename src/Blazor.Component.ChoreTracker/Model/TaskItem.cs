namespace Blazor.Component.ChoreTracker.Model;

public class TaskItem
{
    public required int Id { get; set; }
    public required string Name { get; init; } = string.Empty;
    public required string Room { get; init; } = string.Empty;
    public required string Assignee { get; init; }  = string.Empty;
    public required Frequency Frequency { get; init; } = Frequency.Daily;
    public bool Completed { get; set; }
}

public enum Frequency
{
    Daily = 1,
    Weekly,
    Monthly,
}

internal record GroupedTaskItem(
    string Key,
    IEnumerable<TaskItem> TaskItems)
{
    public int DoneCount => TaskItems.Count(i => i.Completed);
    public int TotalCount => TaskItems.Count();
}