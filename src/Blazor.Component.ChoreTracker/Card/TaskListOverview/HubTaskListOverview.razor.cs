using HubAddTaskItem = Blazor.Component.ChoreTracker.Card.TaskListOverview.Dialog.TaskList.HubAddTaskItem;

namespace Blazor.Component.ChoreTracker.Card.TaskListOverview;

public partial class HubTaskListOverview : ComponentBase
{
    [Inject]
    public required IDialogService DialogService { get; init; }
    
    /// <summary>
    /// The task items
    /// </summary>
    [Parameter, EditorRequired]
    public required IList<TaskItem> TaskItemCollection { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnToggleButtonClick { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnDeleteButtonClick { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnAddButtonClick { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnEditButtonClick { get; init; }
    
    private int _selectedPanelFrequency = 0;
    
    private IEnumerable<GroupedTaskItem> GetGroupedTasks()
    {
        // 1. Determine the filter criteria once
        var filterApplied = Enum.IsDefined(typeof(Frequency), _selectedPanelFrequency);
        var targetFrequency = (Frequency)_selectedPanelFrequency;

        return TaskItemCollection
            // 2. Group the full collection to ensure every Room key exists
            .GroupBy(item => item.Room)
            .Select(grouping => 
            {
                // 3. Filter the items within the group
                var filteredItems = filterApplied 
                    ? grouping.Where(c => c.Frequency == targetFrequency).ToArray()
                    : grouping.ToArray();

                // 4. Return the group, even if filteredItems is empty
                return new GroupedTaskItem(grouping.Key, TaskItems: filteredItems);
            })
            .OrderBy(item => item.Key);
    }

    private void OnPanelIndexChanged(int index) => _selectedPanelFrequency = index;

    private static Color GetFrequencyColor(Frequency frequency) => frequency switch {
        Frequency.Daily => Color.Primary,
        Frequency.Weekly => Color.Secondary,
        _ => Color.Tertiary
    };
    
    private async Task ToggleTaskItemAsync(int id)
    {
        var task = TaskItemCollection.FirstOrDefault(t => t.Id == id);
        task?.Completed = !task.Completed;
        await OnToggleButtonClick.InvokeAsync();
    }
    
    private async Task DeleteTaskItemAsync(TaskItem item)
    {
        TaskItemCollection.Remove(item);
        await OnDeleteButtonClick.InvokeAsync();
    }
    
    // Helper to get unique lists for the Autocomplete
    private IEnumerable<string> GetUniqueRooms() => 
        TaskItemCollection.Select(i => i.Room).Distinct().OrderBy(x => x);

    private IEnumerable<string> GetUniqueAssignees() => 
        TaskItemCollection.Select(i => i.Assignee).Distinct().OrderBy(x => x);

    private async Task OpenTaskDialogAsync(TaskItem? itemToEdit = null)
    {
        var parameters = new DialogParameters<HubAddTaskItem>
        {
            { x => x.ExistingRooms, GetUniqueRooms() },
            { x => x.ExistingAssignees, GetUniqueAssignees() },
            { x => x.TaskItemToEdit, itemToEdit }
        };

        var options = new DialogOptions { CloseOnEscapeKey = true, FullWidth = true };
        string title = itemToEdit == null ? "Add New Task" : "Edit Task";
        
        var dialog = await DialogService.ShowAsync<HubAddTaskItem>(title, parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: TaskItem returnedItem })
        {
            if (itemToEdit == null)
            {
                // CREATE Logic
                int newId = TaskItemCollection.Any()
                    ? TaskItemCollection.Max(t => t.Id) + 1
                    : 1;
                returnedItem.Id = newId;
                TaskItemCollection.Add(returnedItem);
                await OnAddButtonClick.InvokeAsync();
            }
            else
            {
                // UPDATE Logic
                // Since TaskItem is a record (immutable properties), we must replace it in the list
                var index = TaskItemCollection.IndexOf(itemToEdit);
                if (index != -1)
                {
                    TaskItemCollection[index] = returnedItem;
                    await OnEditButtonClick.InvokeAsync();
                }
            }
        }
    }

    private async Task AddTaskItemAsync() => await OpenTaskDialogAsync(null);

    private async Task EditTaskItemAsync(TaskItem item) => await OpenTaskDialogAsync(item);
}