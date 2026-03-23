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

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (!RoleNames.All.Contains(request.Role))
>>>>>>> theirs
=======
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (!RoleNames.All.Contains(request.Role))
>>>>>>> theirs
=======
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (!RoleNames.All.Contains(request.Role))
>>>>>>> theirs
=======
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (!RoleNames.All.Contains(request.Role))
>>>>>>> theirs
        {
            throw new InvalidOperationException("Invalid role selected.");
        }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
        if (!RoleFlowRules.CanAssignRole(creator.Role, targetRole.Name))
        {
            throw new InvalidOperationException("Your role cannot create the selected user type.");
        }

        var email = NormalizeEmail(request.Email);
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
        if (await db.Users.AnyAsync(user => user.Email == email))
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = await CreateUserAsync(request.Name, request.Email, request.Password, request.Role, request.CustomerCode, null);
        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = NormalizeEmail(request.Email);
        var user = await db.Users.FirstOrDefaultAsync(item => item.Email == email && item.IsActive)
            ?? throw new InvalidOperationException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        return BuildAuthResponse(user);
    }

    public Task<ApplicationUser> CreateManagedUserAsync(CreateManagedUserRequest request, ApplicationUser actor)
    {
        ValidateRoleAssignment(actor, request.Role, request.CustomerCode);
        return CreateUserAsync(request.Name, request.Email, request.Password, request.Role, request.CustomerCode, actor);
    }

    private async Task<ApplicationUser> CreateUserAsync(string name, string email, string password, string role, string? customerCode, ApplicationUser? actor)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Name and password are required.");
        }

        if (!RoleNames.All.Contains(role))
        {
            throw new InvalidOperationException("Invalid role selected.");
        }

        var normalizedEmail = NormalizeEmail(email);
        if (await db.Users.AnyAsync(user => user.Email == normalizedEmail))
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        {
            throw new InvalidOperationException("Email already exists.");
        }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
        var customerCode = ResolveCustomerCode(targetRole.Name, request.CustomerCode);
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        var user = new ApplicationUser
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
            Role = request.Role,
            CustomerCode = string.IsNullOrWhiteSpace(request.CustomerCode) ? "GLOBAL" : request.CustomerCode.Trim().ToUpperInvariant()
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        var normalizedCustomerCode = NormalizeCustomerCode(customerCode);
        if (actor is not null)
        {
            ValidateRoleAssignment(actor, role, normalizedCustomerCode);
        }

        var user = new ApplicationUser
        {
            Name = name.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            CustomerCode = normalizedCustomerCode
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours

        return BuildAuthResponse(user);
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
        var email = NormalizeEmail(request.Email);
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
=======
        var email = request.Email.Trim().ToLowerInvariant();
>>>>>>> theirs
        var user = await db.Users.FirstOrDefaultAsync(user => user.Email == email)
            ?? throw new InvalidOperationException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        return user;
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static string NormalizeCustomerCode(string? customerCode) =>
        string.IsNullOrWhiteSpace(customerCode) ? "GLOBAL" : customerCode.Trim().ToUpperInvariant();

    private static void ValidateRoleAssignment(ApplicationUser actor, string targetRole, string customerCode)
    {
        if (actor.Role == RoleNames.Admin)
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        {
            return;
        }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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

        var siteName = template?.SiteName ?? $"{displayName} Portal";
        var siteSlug = await GenerateUniqueSiteSlugAsync(
            template?.SiteSlug,
            siteName,
            normalizedCompanyCode,
            normalizedCustomerCode);

        db.PortalDesigns.Add(new PortalDesign
        {
            CompanyCode = normalizedCompanyCode,
            CustomerCode = normalizedCustomerCode,
            SiteName = siteName,
            SiteSlug = siteSlug,
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

    private async Task<string> GenerateUniqueSiteSlugAsync(
        string? requestedSlug,
        string fallbackName,
        string companyCode,
        string customerCode)
    {
        var baseSlug = NormalizeSiteSlug(requestedSlug, fallbackName, companyCode, customerCode);
        var candidate = baseSlug;
        var suffix = 2;

        while (await db.PortalDesigns.AnyAsync(design => design.SiteSlug == candidate))
        {
            candidate = $"{baseSlug}-{suffix++}";
        }

        return candidate;
    }

    private static string NormalizeSiteSlug(string? requestedSlug, string fallbackName, string companyCode, string customerCode)
    {
        var normalized = Slugify(string.IsNullOrWhiteSpace(requestedSlug) ? fallbackName : requestedSlug);
        if (!string.IsNullOrWhiteSpace(normalized))
        {
            return normalized;
        }

        var fallbackSlug = Slugify($"{companyCode}-{customerCode}");
        return string.IsNullOrWhiteSpace(fallbackSlug)
            ? Guid.NewGuid().ToString("N")
            : fallbackSlug;
    }

    private static string Slugify(string value)
    {
        var builder = new StringBuilder();
        var lastWasDash = false;

        foreach (var character in value.Trim().ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                lastWasDash = false;
            }
            else if (!lastWasDash)
            {
                builder.Append('-');
                lastWasDash = true;
            }
        }

        return builder.ToString().Trim('-');
    }

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

=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        return BuildAuthResponse(user);
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        if (actor.Role == RoleNames.CustomerAdmin)
        {
            if (actor.CustomerCode != customerCode)
            {
                throw new InvalidOperationException("Customer admins can only manage users inside their own customer scope.");
            }

            if (targetRole is not (RoleNames.CustomerAdmin or RoleNames.CustomerEmployee or RoleNames.CustomerUser))
            {
                throw new InvalidOperationException("Customer admins can only create customer roles.");
            }

            return;
        }

        throw new InvalidOperationException("You do not have permission to create users.");
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    }

    private AuthResponse BuildAuthResponse(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role),
            new("customer_code", user.CustomerCode ?? "GLOBAL")
        };

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role),
            new("customer_code", user.CustomerCode)
        };

<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
=======
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Email, user.Role, user.CustomerCode);
    }
>>>>>>> theirs
}
