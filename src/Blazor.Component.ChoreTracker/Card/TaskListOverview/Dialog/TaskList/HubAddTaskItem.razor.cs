namespace Blazor.Component.ChoreTracker.Card.TaskListOverview.Dialog.TaskList;

public partial class HubAddTaskItem : ComponentBase
{
    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; init; }

    [Parameter] public IEnumerable<string> ExistingRooms { get; set; } = [];
    [Parameter] public IEnumerable<string> ExistingAssignees { get; set; } = [];
    [Parameter] public TaskItem? TaskItemToEdit { get; set; }

    private string _taskName = "";
    private string _room = "";
    private string _assignee = "";
    private Frequency _frequency = Frequency.Daily;

    protected override void OnInitialized()
    {
        if (TaskItemToEdit is null) return;
        
        _taskName = TaskItemToEdit.Name;
        _room = TaskItemToEdit.Room;
        _assignee = TaskItemToEdit.Assignee;
        _frequency = TaskItemToEdit.Frequency;
    }

    private Task<IEnumerable<string>> SearchRooms(string value, CancellationToken token)
    {
        return Task.FromResult(string.IsNullOrEmpty(value)
            ? ExistingRooms
            : ExistingRooms.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
    }

    private Task<IEnumerable<string>> SearchAssignees(string value, CancellationToken token)
    {
        return Task.FromResult(string.IsNullOrEmpty(value)
            ? ExistingAssignees
            : ExistingAssignees.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
    }

    private void Cancel() => MudDialog.Cancel();

    private void Submit()
    {
        // If editing, keep ID. If new, ID is 0 (will be set by parent).
        var id = TaskItemToEdit?.Id ?? 0;
        
        var newItem = new TaskItem
        {
            Id = id,
            Name = _taskName,
            Room = _room,
            Assignee = _assignee,
            Frequency = _frequency,
            Completed = TaskItemToEdit?.Completed ?? false 
        };
        
        MudDialog.Close(DialogResult.Ok(newItem));
    }
}