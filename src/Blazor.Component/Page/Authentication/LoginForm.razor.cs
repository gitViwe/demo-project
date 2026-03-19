namespace Blazor.Component.Page.Authentication;

public partial class LoginForm : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts => new();
    
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    [Inject]
    public required IAuthenticationManager AuthenticationManager { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private static string RegisterRoute => HubBlazorPage.Authentication.Register;
    public LoginRequest LoginRequest { get; set; } = new() { Email = "example@email.com", Password = "Password" };
    private bool IsProcessing { get; set; }
    private string IsProcessingMessage { get; set; } = "Processing";
    
    private async Task SubmitAsync()
    {
        IsProcessingMessage = "Processing";
        IsProcessing = true;

        var response = await AuthenticationManager.LoginAsync(LoginRequest, Cts.Token);

        // display response messages
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);

        if (response.Succeeded)
        {
            var url = new Uri(NavigationManager.Uri);
            var query = HttpUtility.ParseQueryString(url.Query);

            var parameters = new Dictionary<string, string>();
            foreach (string key in query)
            {
                parameters[key] = query[key]!;
            }

            if (parameters.TryGetValue("returnUrl", out var path) && false == string.IsNullOrWhiteSpace(path))
            {
                NavigationManager.NavigateTo(path);
            }
            else
            {
                NavigationManager.NavigateTo(HubBlazorPage.Authentication.Account);
            }
        }

        IsProcessing = false;
    }

    private void OnLoadingTimeoutElapsed()
    {
        IsProcessingMessage = "Taking longer than expected";
        Snackbar.Add("We seem to be experiencing some connection issues.",Severity.Warning);
    }
    
    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}