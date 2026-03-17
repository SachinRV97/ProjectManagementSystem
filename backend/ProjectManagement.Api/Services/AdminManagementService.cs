using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Data;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Services;

public class AdminManagementService(AppDbContext db) : IAdminManagementService
{
    public async Task<IReadOnlyList<CompanyListItemResponse>> GetCompaniesAsync(string? search, bool? isLoginAllowed)
    {
        var query = db.Companies
            .AsNoTracking()
            .GroupJoin(
                db.Users.AsNoTracking(),
                company => company.Code,
                user => user.CompanyCode,
                (company, users) => new { Company = company, UserCount = users.Count() });

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim().ToLowerInvariant();
            query = query.Where(item =>
                item.Company.Name.ToLower().Contains(normalizedSearch) ||
                item.Company.Code.ToLower().Contains(normalizedSearch) ||
                item.Company.ContactEmail.ToLower().Contains(normalizedSearch) ||
                (item.Company.ContactPhone ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                (item.Company.City ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                (item.Company.State ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                (item.Company.Country ?? string.Empty).ToLower().Contains(normalizedSearch));
        }

        if (isLoginAllowed.HasValue)
        {
            query = query.Where(item => item.Company.IsLoginAllowed == isLoginAllowed.Value);
        }

        var items = await query
            .OrderBy(item => item.Company.Name)
            .Select(item => new CompanyListItemResponse(
                item.Company.Id,
                item.Company.Name,
                item.Company.Code,
                item.Company.ContactEmail,
                item.Company.ContactPhone,
                item.Company.City,
                item.Company.State,
                item.Company.Country,
                item.Company.IsLoginAllowed,
                item.UserCount,
                item.Company.CreatedAtUtc,
                item.Company.UpdatedAtUtc))
            .ToListAsync();

        return items;
    }

    public async Task<CompanyDetailsResponse> GetCompanyAsync(Guid companyId)
    {
        var company = await db.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == companyId)
            ?? throw new InvalidOperationException("Company was not found.");

        return ToCompanyDetailsResponse(company);
    }

    public async Task<CompanyDetailsResponse> UpdateCompanyAsync(Guid companyId, UpdateCompanyRequest request)
    {
        var company = await db.Companies.FirstOrDefaultAsync(item => item.Id == companyId)
            ?? throw new InvalidOperationException("Company was not found.");

        var newCode = NormalizeCode(request.Code);
        if (string.IsNullOrWhiteSpace(newCode))
        {
            throw new InvalidOperationException("Company code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidOperationException("Company name is required.");
        }

        var normalizedContactEmail = NormalizeEmail(request.ContactEmail);
        if (string.IsNullOrWhiteSpace(normalizedContactEmail))
        {
            throw new InvalidOperationException("Contact email is required.");
        }

        if (company.Code == CompanyCodes.Global)
        {
            if (newCode != CompanyCodes.Global)
            {
                throw new InvalidOperationException("The global company code cannot be changed.");
            }

            if (!request.IsLoginAllowed)
            {
                throw new InvalidOperationException("The global company cannot be blocked.");
            }
        }

        if (!string.Equals(company.Code, newCode, StringComparison.OrdinalIgnoreCase) &&
            await db.Companies.AnyAsync(item => item.Code == newCode))
        {
            throw new InvalidOperationException("Company code already exists.");
        }

        var oldCode = company.Code;
        company.Name = request.Name.Trim();
        company.Code = newCode;
        company.ContactEmail = normalizedContactEmail;
        company.ContactPhone = NormalizeOptional(request.ContactPhone);
        company.Website = NormalizeOptional(request.Website);
        company.AddressLine1 = NormalizeOptional(request.AddressLine1);
        company.AddressLine2 = NormalizeOptional(request.AddressLine2);
        company.City = NormalizeOptional(request.City);
        company.State = NormalizeOptional(request.State);
        company.Country = NormalizeOptional(request.Country);
        company.PostalCode = NormalizeOptional(request.PostalCode);
        company.IsLoginAllowed = request.IsLoginAllowed;
        company.UpdatedAtUtc = DateTime.UtcNow;

        if (!string.Equals(oldCode, newCode, StringComparison.OrdinalIgnoreCase))
        {
            var companyUsers = await db.Users.Where(user => user.CompanyCode == oldCode).ToListAsync();
            foreach (var user in companyUsers)
            {
                user.CompanyCode = newCode;
                user.UpdatedAtUtc = DateTime.UtcNow;
            }

            var companyDesigns = await db.PortalDesigns.Where(design => design.CompanyCode == oldCode).ToListAsync();
            foreach (var design in companyDesigns)
            {
                design.CompanyCode = newCode;
                design.UpdatedAtUtc = DateTime.UtcNow;
            }
        }

        await db.SaveChangesAsync();

        return ToCompanyDetailsResponse(company);
    }

    public async Task<IReadOnlyList<AdminUserListItemResponse>> GetUsersAsync(string? search, string? companyCode, bool? isLoginAllowed)
    {
        var query = db.Users
            .AsNoTracking()
            .Join(
                db.Companies.AsNoTracking(),
                user => user.CompanyCode,
                company => company.Code,
                (user, company) => new { User = user, Company = company });

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim().ToLowerInvariant();
            query = query.Where(item =>
                item.User.Name.ToLower().Contains(normalizedSearch) ||
                item.User.Email.ToLower().Contains(normalizedSearch) ||
                item.User.Role.ToLower().Contains(normalizedSearch) ||
                item.Company.Name.ToLower().Contains(normalizedSearch) ||
                item.Company.Code.ToLower().Contains(normalizedSearch) ||
                (item.User.CustomerCode ?? string.Empty).ToLower().Contains(normalizedSearch));
        }

        if (!string.IsNullOrWhiteSpace(companyCode))
        {
            var normalizedCompanyCode = NormalizeCode(companyCode);
            query = query.Where(item => item.User.CompanyCode == normalizedCompanyCode);
        }

        if (isLoginAllowed.HasValue)
        {
            query = query.Where(item => item.User.IsLoginAllowed == isLoginAllowed.Value);
        }

        var items = await query
            .OrderBy(item => item.Company.Name)
            .ThenBy(item => item.User.Name)
            .Select(item => new AdminUserListItemResponse(
                item.User.Id,
                item.User.Name,
                item.User.Email,
                item.User.Role,
                item.User.CompanyCode,
                item.Company.Name,
                item.User.CustomerCode,
                item.User.IsLoginAllowed,
                item.User.CreatedAtUtc))
            .ToListAsync();

        return items;
    }

    public async Task<AdminUserListItemResponse> UpdateUserLoginStatusAsync(Guid currentUserId, Guid userId, UpdateUserLoginStatusRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(item => item.Id == userId)
            ?? throw new InvalidOperationException("User was not found.");

        if (!request.IsLoginAllowed &&
            user.CompanyCode == CompanyCodes.Global &&
            user.Role == RoleNames.Admin)
        {
            var activeGlobalAdminCount = await db.Users.CountAsync(item =>
                item.CompanyCode == CompanyCodes.Global &&
                item.Role == RoleNames.Admin &&
                item.IsLoginAllowed &&
                item.Id != user.Id);

            if (activeGlobalAdminCount == 0)
            {
                throw new InvalidOperationException("At least one global admin must remain active.");
            }
        }

        if (!request.IsLoginAllowed && user.Id == currentUserId)
        {
            throw new InvalidOperationException("You cannot block your own login from this screen.");
        }

        user.IsLoginAllowed = request.IsLoginAllowed;
        user.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync();

        var company = await db.Companies.AsNoTracking().FirstAsync(item => item.Code == user.CompanyCode);

        return new AdminUserListItemResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.CompanyCode,
            company.Name,
            user.CustomerCode,
            user.IsLoginAllowed,
            user.CreatedAtUtc);
    }

    private static CompanyDetailsResponse ToCompanyDetailsResponse(Company company) =>
        new(
            company.Id,
            company.Name,
            company.Code,
            company.ContactEmail,
            company.ContactPhone,
            company.Website,
            company.AddressLine1,
            company.AddressLine2,
            company.City,
            company.State,
            company.Country,
            company.PostalCode,
            company.IsLoginAllowed,
            company.CreatedAtUtc,
            company.UpdatedAtUtc);

    private static string NormalizeCode(string code) =>
        code.Trim().ToUpperInvariant();

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
