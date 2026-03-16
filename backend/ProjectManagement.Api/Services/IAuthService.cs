using ProjectManagement.Api.Dtos;

namespace ProjectManagement.Api.Services;

public interface IAuthService
{
    Task<IReadOnlyList<RoleResponse>> GetAvailableRolesAsync();
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
