namespace Blazor.Component.Card.ImageHover;

public partial class HubImageHoverItem : ComponentBase
{
    /// <summary>
    /// The image URL.
    /// </summary>
    [Parameter, EditorRequired]
    public required string ImageSrc { get; init; }
    
    /// <summary>
    /// Defines text that can replace the image in the page.
    /// </summary>
    [Parameter, EditorRequired]
    public required string ImageAlt { get; init; }
    
    /// <summary>
    /// Defines text to be displayed when cursor is in hover state
    /// </summary>
    [Parameter, EditorRequired]
    public required string HoverText { get; init; }

    /// <summary>
    /// A bound event handler for the click event.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; init; }
    
    private void OnClickHandler(MouseEventArgs args) => OnClick.InvokeAsync(args);
}