using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Data;

public static class DbSeeder
{
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    public static void Seed(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            db.Users.AddRange(
                new ApplicationUser
                {
                    Name = "Super Admin",
                    Email = "admin@system.local",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = RoleNames.Admin,
                    CustomerCode = "GLOBAL"
                },
                new ApplicationUser
                {
                    Name = "Acme Customer Admin",
                    Email = "customeradmin@acme.local",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = RoleNames.CustomerAdmin,
                    CustomerCode = "ACME"
                });
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        }

        if (!db.PortalDesigns.Any())
        {
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
            db.PortalDesigns.AddRange(CreateDefaultPortal("GLOBAL", "Unified Project Management Portal"), CreateDefaultPortal("ACME", "Acme Employee & Customer Portal"));
>>>>>>> theirs
=======
            db.PortalDesigns.AddRange(CreateDefaultPortal("GLOBAL", "Unified Project Management Portal"), CreateDefaultPortal("ACME", "Acme Employee & Customer Portal"));
>>>>>>> theirs
=======
            db.PortalDesigns.AddRange(CreateDefaultPortal("GLOBAL", "Unified Project Management Portal"), CreateDefaultPortal("ACME", "Acme Employee & Customer Portal"));
>>>>>>> theirs
        }

        db.SaveChanges();
    }
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs

    private static PortalDesign CreateDefaultPortal(string customerCode, string portalName)
    {
        return new PortalDesign
        {
            CustomerCode = customerCode,
            PortalName = portalName,
            HeaderTitle = portalName,
            FooterText = $"© 2026 {portalName}",
            PrimaryColor = customerCode == "GLOBAL" ? "#2563eb" : "#7c3aed",
            SecondaryColor = "#0f172a",
            HeroTitle = $"Manage {customerCode} portal experience in one place",
            HeroSubtitle = "Control header, footer, content sections, and navigation without rebuilding the app.",
            AnnouncementText = "New release notes and onboarding guides can be published from the portal builder.",
            SupportEmail = $"support@{customerCode.ToLowerInvariant()}.local",
            NavigationItems =
            [
                new PortalNavigationItem { Label = "Home", Href = "/", SortOrder = 1 },
                new PortalNavigationItem { Label = "Projects", Href = "/projects", SortOrder = 2 },
                new PortalNavigationItem { Label = "Support", Href = "/support", SortOrder = 3 }
            ],
            ContentSections =
            [
                new PortalContentSection { SectionKey = "hero", Title = "Start your workspace", Body = "Launch customer and employee experiences from a configurable platform.", SortOrder = 1 },
                new PortalContentSection { SectionKey = "features", Title = "Flexible portal features", Body = "Design reusable pages, highlight services, and support onboarding content.", SortOrder = 2 },
                new PortalContentSection { SectionKey = "cta", Title = "Need custom flows?", Body = "Extend this starter with project modules, approvals, billing, and analytics.", SortOrder = 3 }
            ]
        };
    }
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
}
