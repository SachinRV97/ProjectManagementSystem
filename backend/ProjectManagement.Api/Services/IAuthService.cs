using ProjectManagement.Api.Dtos;
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
=======
using ProjectManagement.Api.Models;
>>>>>>> theirs
=======
using ProjectManagement.Api.Models;
>>>>>>> theirs
=======
using ProjectManagement.Api.Models;
>>>>>>> theirs

namespace ProjectManagement.Api.Services;

public interface IAuthService
{
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
>>>>>>> theirs
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
>>>>>>> theirs
    Task<AuthResponse> LoginAsync(LoginRequest request);
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<ApplicationUser> CreateManagedUserAsync(CreateManagedUserRequest request, ApplicationUser actor);
>>>>>>> theirs
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<ApplicationUser> CreateManagedUserAsync(CreateManagedUserRequest request, ApplicationUser actor);
>>>>>>> theirs
=======
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<ApplicationUser> CreateManagedUserAsync(CreateManagedUserRequest request, ApplicationUser actor);
>>>>>>> theirs
}
