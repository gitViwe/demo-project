namespace Blazor.Component.Layout;

public partial class HubAuthenticationLayout : LayoutComponentBase
{
    private bool IsDarkMode { get; set; }
    
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