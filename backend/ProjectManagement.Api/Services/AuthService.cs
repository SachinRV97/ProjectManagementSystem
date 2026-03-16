using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Api.Data;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Services;

public class AuthService(AppDbContext db, IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<IReadOnlyList<RoleResponse>> GetAvailableRolesAsync()
    {
        var roles = await db.Roles
            .AsNoTracking()
            .Where(role => role.IsActive)
            .OrderBy(role => role.DisplayOrder)
            .ThenBy(role => role.Name)
            .ToListAsync();

        return roles.Select(ToRoleResponse).ToList();
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var role = await FindActiveRoleAsync(request.Role);
        if (role is null)
        {
            throw new InvalidOperationException("Invalid role selected.");
        }

        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(user => user.Email == email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var user = new ApplicationUser
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role.Name,
            CustomerCode = string.IsNullOrWhiteSpace(request.CustomerCode) ? "GLOBAL" : request.CustomerCode.Trim().ToUpperInvariant()
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return BuildAuthResponse(user, role);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(user => user.Email == email)
            ?? throw new InvalidOperationException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        var role = await FindActiveRoleAsync(user.Role);
        if (role is null)
        {
            throw new InvalidOperationException("Assigned role is not available.");
        }

        return BuildAuthResponse(user, role);
    }

    private async Task<AppRole?> FindActiveRoleAsync(string roleName)
    {
        var normalizedRoleName = roleName.Trim();

        return await db.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.IsActive && role.Name == normalizedRoleName);
    }

    private AuthResponse BuildAuthResponse(ApplicationUser user, AppRole role)
    {
        var permissions = role.GetPermissions();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role),
            new("customer_code", user.CustomerCode ?? "GLOBAL")
        };

        claims.AddRange(permissions.Select(permission => new Claim(CustomClaimTypes.Permission, permission)));

        if (role.LimitPortalManagementToOwnCustomer)
        {
            claims.Add(new Claim(CustomClaimTypes.PortalScope, PortalScopeValues.OwnCustomer));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            user.Name,
            user.Email,
            user.Role,
            user.CustomerCode,
            permissions,
            role.LimitPortalManagementToOwnCustomer);
    }

    private static RoleResponse ToRoleResponse(AppRole role) =>
        new(
            role.Id,
            role.Name,
            role.Description,
            role.GetPermissions(),
            role.LimitPortalManagementToOwnCustomer);
}
