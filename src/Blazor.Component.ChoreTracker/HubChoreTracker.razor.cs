namespace Blazor.Component.ChoreTracker;

public partial class HubChoreTracker : ComponentBase, IComponentCancellationTokenSource
{
    private const string StorageKey = "ChoreTaskItems";
    private const string ChoreTelemetryName = "Chore.Telemetry";
    public CancellationTokenSource Cts => new();
    
    [Inject]
    public required IJSRuntime JsRuntime { get; init; }
    
    [CascadingParameter]
    public CascadingHubOpenTelemetryManagerContext? TelemetryManagerContext { get; init; }
    
    protected override async Task OnInitializedAsync()
    {
        var savedItems = await JsRuntime.LocalStorageGetAsync<List<TaskItem>>(StorageKey, CancellationToken.None);
        
        if (savedItems != null && savedItems.Any())
        {
            TaskItems = savedItems;
        }
        else
        {
            // Default initial data if storage is empty
            TaskItems =
            [
                new TaskItem { Id = 1, Name = "Wash Dishes", Room = "Kitchen", Frequency = Frequency.Daily, Assignee = "Dad", Completed = false },
                new TaskItem { Id = 2, Name = "Wipe Counters", Room = "Kitchen", Frequency = Frequency.Daily, Assignee = "Mom", Completed = false },
                new TaskItem { Id = 3, Name = "Take out Trash", Room = "Kitchen", Frequency = Frequency.Weekly, Assignee = "Dad", Completed = false },
                new TaskItem { Id = 4, Name = "Vacuum Floors", Room = "Living Area", Frequency = Frequency.Weekly, Assignee = "Kids", Completed = false },
                new TaskItem { Id = 5, Name = "Sweep Patio", Room = "Patio/Garden", Frequency = Frequency.Weekly, Assignee = "Dad", Completed = false },
                new TaskItem { Id = 6, Name = "Deep Clean Fridge", Room = "Kitchen", Frequency = Frequency.Monthly, Assignee = "Dad", Completed = false }
            ];
            await PersistToStorage();
        }
    }

    private List<TaskItem> TaskItems { get; set; } = [];
    
    private int GetPercentage(Frequency frequency) {
        var subset = TaskItems.Where(c => c.Frequency == frequency).ToList();
        var done = subset.Count(c => c.Completed);
        var total = subset.Count;
        return total == 0 ? 0 : (int)Math.Round((double)done / total * 100);
    }
    
    private IEnumerable<KeyValuePair<string, int>> ItemProgressDetail => TaskItems.Select(x => new KeyValuePair<string, int>(x.Frequency.ToString(), GetPercentage(x.Frequency))).DistinctBy(x => x.Key);
    
    private async Task PersistToStorage()
    {
        await JsRuntime.LocalStorageSetAsync(StorageKey, JsonSerializer.Serialize(TaskItems), CancellationToken.None);
    }
    
    private async Task ResetButtonClicked(string frequency)
    {
        foreach (var item in TaskItems.Where(t => t.Frequency.ToString() == frequency))
        {
            item.Completed = false;
        }
        
        await PersistToStorage();

        if (TelemetryManagerContext is not null)
        {
            await TelemetryManagerContext.UpdateMetricAsync(
                new MetricRequest(
                    $"{ChoreTelemetryName}.ResetButtonClicked",
                    1,
                    new Dictionary<string, object>{ { "Frequency", frequency } }),
                Cts.Token);
            
            await TelemetryManagerContext.TrackEventAsync(
                $"{ChoreTelemetryName}",
                "ResetButtonClicked",
                new Dictionary<string, object>{ { "Frequency", frequency } },
                Cts.Token);
        }
    }
    
    private async Task AddButtonClicked()
    {
        await PersistToStorage();
        
        if (TelemetryManagerContext is not null)
        {
            await TelemetryManagerContext.UpdateMetricAsync(
                new MetricRequest($"{ChoreTelemetryName}.AddButtonClicked", 1),
                Cts.Token);
            
            await TelemetryManagerContext.TrackEventAsync(
                $"{ChoreTelemetryName}",
                "AddButtonClicked",
                [],
                Cts.Token);
        }
    }
    
    private async Task EditButtonClicked()
    {
        await PersistToStorage();
        
        if (TelemetryManagerContext is not null)
        {
            await TelemetryManagerContext.UpdateMetricAsync(
                new MetricRequest($"{ChoreTelemetryName}.EditButtonClicked", 1),
                Cts.Token);
            
            await TelemetryManagerContext.TrackEventAsync(
                $"{ChoreTelemetryName}",
                "EditButtonClicked",
                [],
                Cts.Token);
        }
    }
    
    private async Task DeleteButtonClicked()
    {
        await PersistToStorage();
        
        if (TelemetryManagerContext is not null)
        {
            await TelemetryManagerContext.UpdateMetricAsync(
                new MetricRequest($"{ChoreTelemetryName}.DeleteButtonClicked", 1),
                Cts.Token);
            
            await TelemetryManagerContext.TrackEventAsync(
                $"{ChoreTelemetryName}",
                "DeleteButtonClicked",
                [],
                Cts.Token);
        }
    }
    
    private async Task ToggleButtonClicked()
    {
        await PersistToStorage();
        
        if (TelemetryManagerContext is not null)
        {
            await TelemetryManagerContext.UpdateMetricAsync(
                new MetricRequest($"{ChoreTelemetryName}.ToggleButtonClicked", 1),
                Cts.Token);
            
            await TelemetryManagerContext.TrackEventAsync(
                $"{ChoreTelemetryName}",
                "ToggleButtonClicked",
                [],
                Cts.Token);
        }
    }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}