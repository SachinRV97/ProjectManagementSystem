namespace ProjectManagement.Api.Models;

public class ApplicationUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = RoleNames.CustomerUser;
<<<<<<< ours
<<<<<<< ours
    public string CompanyCode { get; set; } = CompanyCodes.Global;
    public string? CustomerCode { get; set; }
    public bool IsLoginAllowed { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
=======
    public string? CustomerCode { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
>>>>>>> theirs
=======
    public string? CustomerCode { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
>>>>>>> theirs
}
