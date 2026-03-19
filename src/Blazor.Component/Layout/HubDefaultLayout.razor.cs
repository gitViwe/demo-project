namespace Blazor.Component.Layout;

public partial class HubDefaultLayout : LayoutComponentBase
{
    private bool IsDrawerOpen { get; set; } = true;
    private bool IsDarkMode { get; set; }
    private void ToggleDrawer() => IsDrawerOpen = !IsDrawerOpen;
    
    private MudThemeProvider? MudThemeProviderReference { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && MudThemeProviderReference is not null)
        {
            var isSystemDarkMode = await MudThemeProviderReference.GetSystemDarkModeAsync();
            DarkModeChanged(isSystemDarkMode);
        }
    }

    private void DarkModeChanged(bool newValue)
    {
        IsDarkMode = newValue;
        StateHasChanged();
    }
}