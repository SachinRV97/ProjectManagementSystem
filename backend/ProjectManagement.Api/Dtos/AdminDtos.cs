namespace ProjectManagement.Api.Dtos;

public record CompanyListItemResponse(
    Guid Id,
    string Name,
    string Code,
    string ContactEmail,
    string? ContactPhone,
    string? City,
    string? State,
    string? Country,
    bool IsLoginAllowed,
    int UserCount,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public record CompanyDetailsResponse(
    Guid Id,
    string Name,
    string Code,
    string ContactEmail,
    string? ContactPhone,
    string? Website,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool IsLoginAllowed,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public record UpdateCompanyRequest(
    string Name,
    string Code,
    string ContactEmail,
    string? ContactPhone,
    string? Website,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    bool IsLoginAllowed);

public record AdminUserListItemResponse(
    Guid Id,
    string Name,
    string Email,
    string Role,
    string CompanyCode,
    string CompanyName,
    string? CustomerCode,
    bool IsLoginAllowed,
    DateTime CreatedAtUtc);

public record UpdateUserLoginStatusRequest(bool IsLoginAllowed);
