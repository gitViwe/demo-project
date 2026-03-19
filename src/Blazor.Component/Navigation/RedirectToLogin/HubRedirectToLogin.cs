namespace Blazor.Component.Navigation.RedirectToLogin;

public sealed class HubRedirectToLogin : ComponentBase
{
    [Inject]
    public required NavigationManager Navigation { get; init; }

    protected override void OnInitialized()
    {
        Navigation.NavigateTo($"{HubBlazorPage.Authentication.Login}?returnUrl={Uri.EscapeDataString(new Uri(Navigation.Uri).PathAndQuery)}");
    }
}