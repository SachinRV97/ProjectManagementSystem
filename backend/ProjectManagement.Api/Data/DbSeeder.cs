using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            db.Users.Add(new ApplicationUser
            {
                Name = "Super Admin",
                Email = "admin@system.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = RoleNames.Admin,
                CustomerCode = "GLOBAL"
            });
        }

        if (!db.PortalDesigns.Any())
        {
            db.PortalDesigns.Add(new PortalDesign
            {
                CustomerCode = "GLOBAL",
                HeaderTitle = "Unified Project Management Portal",
                FooterText = "© 2026 Unified Portal",
                PrimaryColor = "#1d4ed8",
                AccentColor = "#0f172a"
            });
        }

        db.SaveChanges();
    }
}
