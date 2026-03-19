namespace Blazor.Shared.Interface;

public interface IAuthenticationManager
{
    Task<IResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<IResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<ITypedResponse<UserDetailResponse>> GetUserDetailAsync(CancellationToken cancellationToken);
    Task<IResponse> UpdateDetailsAsync(UpdateUserRequest request, CancellationToken cancellationToken);
}