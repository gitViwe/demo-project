namespace Blazor.Component.Card.ImageHover;

public partial class HubImageHover : ComponentBase
{
    /// <summary>
    /// The <see cref="HubImageHoverItem"/>(s) to render
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }
}