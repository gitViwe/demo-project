namespace Blazor.Component.Typography.TextFadeIn;

public partial class HubTextFadeIn : ComponentBase
{
    /// <summary>
    /// The title text
    /// </summary>
    [Parameter, EditorRequired]
    public required string TitleText { get; set; }
    
    /// <summary>
    /// The sub title tex
    /// </summary>
    [Parameter, EditorRequired]
    public required string SubTitleText { get; set; }
}