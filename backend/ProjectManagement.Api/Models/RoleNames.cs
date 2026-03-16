namespace ProjectManagement.Api.Models;

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string PortalAdmin = "Portal-Admin";
    public const string PortalEmployee = "Portal-Employee";
    public const string CustomerAdmin = "Customer-Admin";
    public const string CustomerEmployee = "Customer-Employee";
    public const string CustomerUser = "Customer-User";

    public static readonly string[] All =
    [
        Admin,
        PortalAdmin,
        PortalEmployee,
        CustomerAdmin,
        CustomerEmployee,
        CustomerUser
    ];
}
