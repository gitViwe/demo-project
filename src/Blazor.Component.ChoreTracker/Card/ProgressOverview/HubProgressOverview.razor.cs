namespace Blazor.Component.ChoreTracker.Card.ProgressOverview;

public partial class HubProgressOverview : ComponentBase
{
    /// <summary>
    /// The progress card title
    /// </summary>
    [Parameter, EditorRequired]
    public required string Title { get; init; }
    
    /// <summary>
    /// The progress card description
    /// </summary>
    [Parameter, EditorRequired]
    public required string Description { get; init; }
    
    /// <summary>
    /// The progress items detail where the key is the name of the item and value is the progress percentage
    /// </summary>
    [Parameter, EditorRequired]
    public required IEnumerable<KeyValuePair<string, int>> ItemProgressDetail { get; init; }
    
    /// <summary>
    /// The progress reset button action with the current Key value of <see cref="ItemProgressDetail"/> being reset
    /// </summary>
    [Parameter, EditorRequired]
    public EventCallback<string> OnResetButtonClick { get; init; }
    
    private static Color GetProgressColor(int percent) => percent switch {
        < 30 => Color.Error,
        < 70 => Color.Warning,
        _ => Color.Success
    };
}