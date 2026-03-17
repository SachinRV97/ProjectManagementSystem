using ProjectManagement.Api.Dtos;

namespace ProjectManagement.Api.Services;

public interface IAdminManagementService
{
    Task<IReadOnlyList<CompanyListItemResponse>> GetCompaniesAsync(string? search, bool? isLoginAllowed);
    Task<CompanyDetailsResponse> GetCompanyAsync(Guid companyId);
    Task<CompanyDetailsResponse> UpdateCompanyAsync(Guid companyId, UpdateCompanyRequest request);
    Task<IReadOnlyList<AdminUserListItemResponse>> GetUsersAsync(string? search, string? companyCode, bool? isLoginAllowed);
    Task<AdminUserListItemResponse> UpdateUserLoginStatusAsync(Guid currentUserId, Guid userId, UpdateUserLoginStatusRequest request);
}
