using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Data;

public static class DbSeeder
{
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    private static readonly (string Name, string Description, bool CanManagePortal, bool CanManageEmployees, bool CanManageCustomers, bool LimitPortalManagementToOwnCustomer, int DisplayOrder)[] DefaultRoles =
    [
        (RoleNames.Admin, "All access", true, true, true, false, 1),
        (RoleNames.PortalAdmin, "Manage portal for employees and customers", true, false, false, false, 2),
        (RoleNames.PortalEmployee, "Manage customer records", false, false, true, false, 3),
        (RoleNames.CustomerAdmin, "Manage employees and users", false, true, false, false, 4),
        (RoleNames.CustomerEmployee, "Change portal design and content for their own customer scope", true, false, false, true, 5),
        (RoleNames.CustomerUser, "Login and normal application usage", false, false, false, false, 6)
    ];

    public static void Seed(AppDbContext db)
    {
        if (!db.Companies.Any(company => company.Code == CompanyCodes.Global))
        {
            db.Companies.Add(new Company
            {
                Name = "Global System",
                Code = CompanyCodes.Global,
                ContactEmail = "admin@system.local"
            });
        }

        foreach (var defaultRole in DefaultRoles)
        {
            if (db.Roles.Any(role => role.Name == defaultRole.Name))
            {
                continue;
            }

            db.Roles.Add(new AppRole
            {
                Name = defaultRole.Name,
                Description = defaultRole.Description,
                CanManagePortal = defaultRole.CanManagePortal,
                CanManageEmployees = defaultRole.CanManageEmployees,
                CanManageCustomers = defaultRole.CanManageCustomers,
                LimitPortalManagementToOwnCustomer = defaultRole.LimitPortalManagementToOwnCustomer,
                DisplayOrder = defaultRole.DisplayOrder
            });
        }

=======
    public static void Seed(AppDbContext db)
    {
>>>>>>> theirs
=======
    public static void Seed(AppDbContext db)
    {
>>>>>>> theirs
=======
    public static void Seed(AppDbContext db)
    {
>>>>>>> theirs
=======
    public static void Seed(AppDbContext db)
    {
>>>>>>> theirs
        if (!db.Users.Any())
        {
            db.Users.Add(new ApplicationUser
            {
                Name = "Super Admin",
                Email = "admin@system.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = RoleNames.Admin,
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
                CompanyCode = CompanyCodes.Global,
                CustomerCode = CompanyCodes.Global
=======
                CustomerCode = "GLOBAL"
>>>>>>> theirs
=======
                CustomerCode = "GLOBAL"
>>>>>>> theirs
=======
                CustomerCode = "GLOBAL"
>>>>>>> theirs
=======
                CustomerCode = "GLOBAL"
>>>>>>> theirs
            });
        }

        if (!db.PortalDesigns.Any())
        {
            db.PortalDesigns.Add(new PortalDesign
            {
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
                CompanyCode = CompanyCodes.Global,
                CustomerCode = CompanyCodes.Global,
                SiteName = "Unified Project Management Portal",
                SiteSlug = "unified-project-management-portal",
=======
                CustomerCode = "GLOBAL",
>>>>>>> theirs
=======
                CustomerCode = "GLOBAL",
>>>>>>> theirs
=======
                CustomerCode = "GLOBAL",
>>>>>>> theirs
=======
                CustomerCode = "GLOBAL",
>>>>>>> theirs
                HeaderTitle = "Unified Project Management Portal",
                FooterText = "© 2026 Unified Portal",
                PrimaryColor = "#1d4ed8",
                AccentColor = "#0f172a"
            });
        }

        db.SaveChanges();
    }
}
