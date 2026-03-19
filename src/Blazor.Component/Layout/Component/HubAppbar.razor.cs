namespace Blazor.Component.Layout.Component;

public partial class HubAppbar : ComponentBase
{
    [Parameter, EditorRequired]
    public EventCallback<MouseEventArgs> OnDrawerToggle { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback<bool> OnDarkModeToggle { get; init; }
    
    [Parameter, EditorRequired]
    public bool IsDarkMode { get; set; }
    
    [Parameter, EditorRequired]
    public required string LogoIcon { get; init; }
    
    [Parameter, EditorRequired]
    public required string LogoDescription { get; init; }

    private void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        OnDarkModeToggle.InvokeAsync(IsDarkMode);
    }
    private string DarkModeIcon => IsDarkMode ? Icons.Material.Filled.WbSunny : Icons.Material.Filled.Nightlight;
    private string DarkModeDescription => IsDarkMode ? "Switch to Light mode" : "Switch to Dark mode";
}