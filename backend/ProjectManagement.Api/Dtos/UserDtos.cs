namespace ProjectManagement.Api.Dtos;

public record UserListItemResponse(Guid Id, string Name, string Email, string Role, string CustomerCode, bool IsActive, DateTime CreatedAtUtc);
public record CreateManagedUserRequest(string Name, string Email, string Password, string Role, string CustomerCode);
