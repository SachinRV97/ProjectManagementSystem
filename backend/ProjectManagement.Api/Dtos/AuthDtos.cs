namespace ProjectManagement.Api.Dtos;

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
public record RegisterCompanyRequest(
    string CompanyName,
    string CompanyCode,
    string ContactEmail,
    string? ContactPhone,
    string? Website,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string AdminName,
    string AdminEmail,
    string AdminPassword);

public record RegisterUserRequest(
    string Name,
    string Email,
    string Password,
    string Role,
    string? CustomerCode);

public record LoginRequest(string Email, string Password);

public record AuthResponse(
    string Token,
    string Name,
    string Email,
    string Role,
    string CompanyName,
    string CompanyCode,
    bool IsGlobalAdmin,
    string? CustomerCode,
    string[] Permissions,
    bool LimitPortalManagementToOwnCustomer);

public record RegisteredUserResponse(
    Guid Id,
    string Name,
    string Email,
    string Role,
    string CompanyCode,
    string? CustomerCode);

public record RoleResponse(
    Guid Id,
    string Name,
    string? Description,
    string[] Permissions,
    bool LimitPortalManagementToOwnCustomer);
=======
public record RegisterRequest(string Name, string Email, string Password, string Role, string? CustomerCode);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Name, string Email, string Role, string? CustomerCode);
>>>>>>> theirs
=======
public record RegisterRequest(string Name, string Email, string Password, string Role, string? CustomerCode);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Name, string Email, string Role, string? CustomerCode);
>>>>>>> theirs
=======
public record RegisterRequest(string Name, string Email, string Password, string Role, string? CustomerCode);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Name, string Email, string Role, string? CustomerCode);
>>>>>>> theirs
=======
public record RegisterRequest(string Name, string Email, string Password, string Role, string? CustomerCode);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Name, string Email, string Role, string? CustomerCode);
>>>>>>> theirs
