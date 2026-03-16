namespace ProjectManagement.Api.Models;

public class AppRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool CanManagePortal { get; set; }
    public bool CanManageEmployees { get; set; }
    public bool CanManageCustomers { get; set; }
    public bool LimitPortalManagementToOwnCustomer { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string[] GetPermissions()
    {
        var permissions = new List<string>();

        if (CanManagePortal)
        {
            permissions.Add(PermissionNames.ManagePortal);
        }

        if (CanManageEmployees)
        {
            permissions.Add(PermissionNames.ManageEmployees);
        }

        if (CanManageCustomers)
        {
            permissions.Add(PermissionNames.ManageCustomers);
        }

        return permissions.ToArray();
    }
}
