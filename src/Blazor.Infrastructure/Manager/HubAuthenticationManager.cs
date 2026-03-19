namespace Blazor.Infrastructure.Manager;

internal class HubAuthenticationManager(
    IJSRuntime jsRuntime,
    IGatewayClient gateway,
    HubAuthenticationStateProvider stateProvider) : IAuthenticationManager
{
    public async Task<IResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.PostAsJsonAsync("account/register", request, cancellationToken);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>(token: cancellationToken);

            if (response is null)
            {
                return Response.Fail("Registration failed.");
            }

            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthToken, response.Token, cancellationToken);
            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthRefreshToken, response.RefreshToken, cancellationToken);

            // update the authentication state
            await stateProvider.StateChangedAsync();

            return Response.Success("Registration successful.");
        }

        return Response.Fail("Registration failed.");
    }
    
    public async Task<IResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.PostAsJsonAsync("account/login", request, cancellationToken);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>(token: cancellationToken);

            if (response is null)
            {
                return Response.Fail("Login failed.");
            }

            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthToken, response.Token, cancellationToken);
            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthRefreshToken, response.RefreshToken, cancellationToken);

            // update the authentication state
            await stateProvider.StateChangedAsync();

            return Response.Success("Login successful.");
        }

        return Response.Fail("Login failed.");
    }
    
    public async Task<ITypedResponse<UserDetailResponse>> GetUserDetailAsync(CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.GetAsync("account/detail", cancellationToken);

        if (false == result.IsSuccessStatusCode)
        {
            return TypedResponse<UserDetailResponse>.Fail("Failed to retrieve user details.");
        }

        var response = await result.ToResponseAsync<UserDetailResponse>(token: cancellationToken);

        if (response is null)
        {
            return TypedResponse<UserDetailResponse>.Fail("Failed to retrieve user details.");
        }
        
        return TypedResponse<UserDetailResponse>.Success("User details retrieved.", response);
    }

    public async Task<IResponse> UpdateDetailsAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.PutAsJsonAsync("account/detail", request, cancellationToken);

        if (result.IsSuccessStatusCode)
        {
            return Response.Success("Update successful.");
        }

        return Response.Fail("Update failed.");
    }
}