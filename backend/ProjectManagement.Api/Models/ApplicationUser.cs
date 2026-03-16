namespace ProjectManagement.Api.Models;

public class ApplicationUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = RoleNames.CustomerUser;
    public string? CustomerCode { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
