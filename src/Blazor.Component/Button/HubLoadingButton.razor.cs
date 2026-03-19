namespace Blazor.Component.Button;

public partial class HubLoadingButton
{
    /// <summary>
    /// The button Type (Button, Submit, Refresh)
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.ClickAction)]
    public ButtonType ButtonType { get; set; }
    
    /// <summary>
    /// Occurs when this button has been clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// The variant to use.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Variant Variant { get; set; } = Variant.Text;

    /// <summary>
    /// The color of the component. It supports the theme colours.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Color Color { get; set; } = Color.Default;

    /// <summary>
    /// The Size of the component.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Size Size { get; set; } = Size.Medium;

    /// <summary>
    /// If true, the button will be disabled.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Behavior)]
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, the button will take up 100% of available width.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public bool FullWidth { get; set; }

    /// <summary>
    /// If true, the button will display the loading fragment.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Behavior)]
    public bool Loading { get; set; }

    /// <summary>
    /// The text displayed when button is loading.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Behavior)]
    public string LoadingMessage { get; set; } = "Loading";

    /// <summary>
    /// Custom loader content.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// Custom loader content.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Behavior)]
    public TimeSpan? LoadingTimeout { get; set; }

    /// <summary>
    /// Fires when loading timeout has elapsed.
    /// </summary>
    [Parameter]
    public EventCallback LoadingTimeoutElapsed { get; set; }

    /// <summary>
    /// The child content.
    /// </summary>
    [Parameter, EditorRequired]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public required RenderFragment ChildContent { get; set; }

    private bool _loadingTimeoutElapsedInvoked;

    protected override void OnParametersSet()
    {
        if (Loading && LoadingTimeout.HasValue && false == _loadingTimeoutElapsedInvoked)
        {
            _ = StartTimerAsync();
        }
    }

    private async Task StartTimerAsync()
    {
        await Task.Delay(LoadingTimeout!.Value);
        _loadingTimeoutElapsedInvoked = true;
        if (Loading)
        {
            await LoadingTimeoutElapsed.InvokeAsync(); 
        }
    }
}