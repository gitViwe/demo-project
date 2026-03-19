namespace Blazor.Component.Card.ImageDetailHover;

public partial class HubImageDetailHover : ComponentBase
{
    /// <summary>
    /// The card detail title
    /// </summary>
    [Parameter, EditorRequired]
    public required string Title { get; init; }
    
    /// <summary>
    /// The card button text
    /// </summary>
    [Parameter]
    public string? ButtonText { get; init; }
    
    /// <summary>
    /// The card button action
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnButtonClick { get; init; }
    
    /// <summary>
    /// The card detail description
    /// </summary>
    [Parameter, EditorRequired]
    public required string Description { get; init; }
    
    /// <summary>
    /// The card detail background image
    /// </summary>
    [Parameter, EditorRequired]
    public required string ImageSrc { get; init; }
}