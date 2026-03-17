namespace ProjectManagement.Api.Models;

public static class RoleFlowRules
{
    private static readonly Dictionary<string, string[]> AssignableRolesByCreator = new(StringComparer.OrdinalIgnoreCase)
    {
        [RoleNames.Admin] =
        [
            RoleNames.Admin,
            RoleNames.PortalAdmin,
            RoleNames.PortalEmployee,
            RoleNames.CustomerAdmin,
            RoleNames.CustomerEmployee
        ],
        [RoleNames.PortalAdmin] =
        [
            RoleNames.PortalEmployee,
            RoleNames.CustomerAdmin,
            RoleNames.CustomerEmployee
        ],
        [RoleNames.PortalEmployee] =
        [
            RoleNames.CustomerAdmin,
            RoleNames.CustomerEmployee
        ]
    };

    public static bool IsCompanyRole(string roleName) =>
        roleName is RoleNames.Admin or RoleNames.PortalAdmin or RoleNames.PortalEmployee;

    public static bool IsCustomerRole(string roleName) =>
        roleName is RoleNames.CustomerAdmin or RoleNames.CustomerEmployee or RoleNames.CustomerUser;

    public static IReadOnlyList<string> GetAssignableRoles(string creatorRole)
    {
        return AssignableRolesByCreator.TryGetValue(creatorRole, out var roles)
            ? roles
            : [];
    }

    public static bool CanAssignRole(string creatorRole, string targetRole) =>
        GetAssignableRoles(creatorRole).Contains(targetRole, StringComparer.OrdinalIgnoreCase);
}
