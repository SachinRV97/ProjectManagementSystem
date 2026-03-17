using ProjectManagement.Api.Dtos;

namespace ProjectManagement.Api.Services;

public interface IAuthService
{
    Task<IReadOnlyList<RoleResponse>> GetAvailableRolesAsync();
    Task<IReadOnlyList<RoleResponse>> GetAssignableRolesAsync(Guid creatorUserId);
    Task<AuthResponse> RegisterCompanyAsync(RegisterCompanyRequest request);
    Task<RegisteredUserResponse> RegisterUserAsync(Guid creatorUserId, RegisterUserRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
