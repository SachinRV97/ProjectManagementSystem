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

    public async Task<IReadOnlyList<RoleResponse>> GetAssignableRolesAsync(Guid creatorUserId)
    {
        var creator = await FindUserAsync(creatorUserId)
            ?? throw new InvalidOperationException("Current user was not found.");

        var assignableRoles = RoleFlowRules.GetAssignableRoles(creator.Role);
        var roles = await db.Roles
            .AsNoTracking()
            .Where(role => role.IsActive && assignableRoles.Contains(role.Name))
            .OrderBy(role => role.DisplayOrder)
            .ThenBy(role => role.Name)
            .ToListAsync();

        return roles.Select(ToRoleResponse).ToList();
    }

    public async Task<AuthResponse> RegisterCompanyAsync(RegisterCompanyRequest request)
    {
        var adminRole = await FindActiveRoleAsync(RoleNames.Admin)
            ?? throw new InvalidOperationException("Admin role is not configured.");

        var companyCode = NormalizeCode(request.CompanyCode);
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new InvalidOperationException("Company code is required.");
        }

        var contactEmail = NormalizeEmail(request.ContactEmail);
        if (string.IsNullOrWhiteSpace(contactEmail))
        {
            throw new InvalidOperationException("Contact email is required.");
        }

        if (await db.Companies.AnyAsync(company => company.Code == companyCode))
        {
            throw new InvalidOperationException("Company code already exists.");
        }

        var email = NormalizeEmail(request.AdminEmail);
        if (await db.Users.AnyAsync(user => user.Email == email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var company = new Company
        {
            Name = request.CompanyName.Trim(),
            Code = companyCode,
            ContactEmail = contactEmail,
            ContactPhone = NormalizeOptional(request.ContactPhone),
            Website = NormalizeOptional(request.Website),
            AddressLine1 = NormalizeOptional(request.AddressLine1),
            AddressLine2 = NormalizeOptional(request.AddressLine2),
            City = NormalizeOptional(request.City),
            State = NormalizeOptional(request.State),
            Country = NormalizeOptional(request.Country),
            PostalCode = NormalizeOptional(request.PostalCode)
        };

        var user = new ApplicationUser
        {
            Name = request.AdminName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.AdminPassword),
            Role = adminRole.Name,
            CompanyCode = company.Code,
            CustomerCode = CompanyCodes.Global
        };

        db.Companies.Add(company);
        db.Users.Add(user);
        await EnsurePortalDesignAsync(company.Code, CompanyCodes.Global, company.Name);
        await db.SaveChangesAsync();

        return BuildAuthResponse(user, adminRole, company);
    }

    public async Task<RegisteredUserResponse> RegisterUserAsync(Guid creatorUserId, RegisterUserRequest request)
    {
        var creator = await FindUserAsync(creatorUserId)
            ?? throw new InvalidOperationException("Current user was not found.");

        var targetRole = await FindActiveRoleAsync(request.Role);
        if (targetRole is null)
        {
            throw new InvalidOperationException("Invalid role selected.");
        }

        if (!RoleFlowRules.CanAssignRole(creator.Role, targetRole.Name))
        {
            throw new InvalidOperationException("Your role cannot create the selected user type.");
        }

        var email = NormalizeEmail(request.Email);
        if (await db.Users.AnyAsync(user => user.Email == email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var customerCode = ResolveCustomerCode(targetRole.Name, request.CustomerCode);
        var user = new ApplicationUser
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = targetRole.Name,
            CompanyCode = creator.CompanyCode,
            CustomerCode = customerCode
        };

        db.Users.Add(user);

        if (RoleFlowRules.IsCustomerRole(targetRole.Name))
        {
            await EnsurePortalDesignAsync(creator.CompanyCode, customerCode, customerCode);
        }

        await db.SaveChangesAsync();

        return new RegisteredUserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.CompanyCode,
            user.CustomerCode);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = NormalizeEmail(request.Email);
        var user = await db.Users.FirstOrDefaultAsync(user => user.Email == email)
            ?? throw new InvalidOperationException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        if (!user.IsLoginAllowed)
        {
            throw new InvalidOperationException("Your login has been blocked by the administrator.");
        }

        var role = await FindActiveRoleAsync(user.Role);
        if (role is null)
        {
            throw new InvalidOperationException("Assigned role is not available.");
        }

        var company = await FindCompanyAsync(user.CompanyCode)
            ?? throw new InvalidOperationException("Assigned company is not available.");

        if (!company.IsLoginAllowed)
        {
            throw new InvalidOperationException("Your company login has been blocked by the administrator.");
        }

        return BuildAuthResponse(user, role, company);
    }

    private async Task<AppRole?> FindActiveRoleAsync(string roleName)
    {
        var normalizedRoleName = roleName.Trim();

        return await db.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.IsActive && role.Name == normalizedRoleName);
    }

    private async Task<ApplicationUser?> FindUserAsync(Guid userId)
    {
        return await db.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    private async Task<Company?> FindCompanyAsync(string companyCode)
    {
        var normalizedCompanyCode = NormalizeCode(companyCode);

        return await db.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(company => company.Code == normalizedCompanyCode);
    }

    private async Task EnsurePortalDesignAsync(string companyCode, string customerCode, string displayName)
    {
        var normalizedCompanyCode = NormalizeCode(companyCode);
        var normalizedCustomerCode = NormalizeCode(customerCode);

        if (await db.PortalDesigns.AnyAsync(design =>
            design.CompanyCode == normalizedCompanyCode &&
            design.CustomerCode == normalizedCustomerCode))
        {
            return;
        }

        var template = await db.PortalDesigns
            .AsNoTracking()
            .FirstOrDefaultAsync(design =>
                design.CompanyCode == normalizedCompanyCode &&
                design.CustomerCode == CompanyCodes.Global)
            ?? await db.PortalDesigns
                .AsNoTracking()
                .FirstOrDefaultAsync(design =>
                    design.CompanyCode == CompanyCodes.Global &&
                    design.CustomerCode == CompanyCodes.Global);

        db.PortalDesigns.Add(new PortalDesign
        {
            CompanyCode = normalizedCompanyCode,
            CustomerCode = normalizedCustomerCode,
            HeaderTitle = template?.HeaderTitle ?? $"{displayName} Portal",
            FooterText = template?.FooterText ?? $"Copyright {DateTime.UtcNow.Year} {displayName}",
            PrimaryColor = template?.PrimaryColor ?? "#1d4ed8",
            AccentColor = template?.AccentColor ?? "#0f172a",
            ShowAnnouncements = template?.ShowAnnouncements ?? true,
            AnnouncementText = template?.AnnouncementText ?? "Welcome to your workspace",
            PageConfigurationsJson = template?.PageConfigurationsJson
        });
    }

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();

    private static string NormalizeCode(string code) =>
        code.Trim().ToUpperInvariant();

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string ResolveCustomerCode(string roleName, string? requestedCustomerCode)
    {
        if (RoleFlowRules.IsCompanyRole(roleName))
        {
            return CompanyCodes.Global;
        }

        var normalizedCustomerCode = NormalizeCode(requestedCustomerCode ?? string.Empty);
        if (string.IsNullOrWhiteSpace(normalizedCustomerCode))
        {
            throw new InvalidOperationException("Customer code is required for customer users.");
        }

        return normalizedCustomerCode;
    }

    private AuthResponse BuildAuthResponse(ApplicationUser user, AppRole role, Company company)
    {
        var permissions = role.GetPermissions();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role),
            new(CustomClaimTypes.CompanyCode, user.CompanyCode),
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
            company.Name,
            user.CompanyCode,
            string.Equals(user.CompanyCode, CompanyCodes.Global, StringComparison.OrdinalIgnoreCase)
                && string.Equals(user.Role, RoleNames.Admin, StringComparison.OrdinalIgnoreCase),
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
