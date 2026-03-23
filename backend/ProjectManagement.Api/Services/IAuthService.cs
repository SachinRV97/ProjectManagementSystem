using ProjectManagement.Api.Dtos;

namespace ProjectManagement.Api.Services;

public interface IAuthService
{
<<<<<<< ours
<<<<<<< ours
    Task<IReadOnlyList<RoleResponse>> GetAvailableRolesAsync();
    Task<IReadOnlyList<RoleResponse>> GetAssignableRolesAsync(Guid creatorUserId);
    Task<AuthResponse> RegisterCompanyAsync(RegisterCompanyRequest request);
    Task<RegisteredUserResponse> RegisterUserAsync(Guid creatorUserId, RegisterUserRequest request);
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
>>>>>>> theirs
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
>>>>>>> theirs
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
