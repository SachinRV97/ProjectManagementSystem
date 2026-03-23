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
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs

    public static readonly IReadOnlyDictionary<string, string[]> Capabilities = new Dictionary<string, string[]>
    {
        [Admin] = ["Full platform access", "Manage portals", "Manage customers", "Manage all users"],
        [PortalAdmin] = ["Configure employee/customer portals", "Review portal layouts", "Oversee experience consistency"],
        [PortalEmployee] = ["Manage customer records", "Review customer onboarding status"],
        [CustomerAdmin] = ["Manage employees and end users", "Review tenant access"],
        [CustomerEmployee] = ["Change portal design", "Update content blocks and menu links"],
        [CustomerUser] = ["Register", "Login", "Use the customer portal"]
    };
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
}
