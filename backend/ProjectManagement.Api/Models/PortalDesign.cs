namespace ProjectManagement.Api.Models;

public class PortalDesign
{
    public Guid Id { get; set; } = Guid.NewGuid();
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    public string CompanyCode { get; set; } = "GLOBAL";
    public string CustomerCode { get; set; } = "GLOBAL";
    public string SiteName { get; set; } = "Unified Project Management Portal";
    public string SiteSlug { get; set; } = "unified-project-management-portal";
=======
    public string CustomerCode { get; set; } = "GLOBAL";
>>>>>>> theirs
=======
    public string CustomerCode { get; set; } = "GLOBAL";
>>>>>>> theirs
=======
    public string CustomerCode { get; set; } = "GLOBAL";
>>>>>>> theirs
=======
    public string CustomerCode { get; set; } = "GLOBAL";
>>>>>>> theirs
    public string HeaderTitle { get; set; } = "Project Management Portal";
    public string FooterText { get; set; } = "© 2026 Project Management";
    public string PrimaryColor { get; set; } = "#1d4ed8";
    public string AccentColor { get; set; } = "#0f172a";
    public bool ShowAnnouncements { get; set; } = true;
    public string? AnnouncementText { get; set; } = "Welcome to your workspace";
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    public string? PageConfigurationsJson { get; set; }
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    public string CustomerCode { get; set; } = "GLOBAL";
    public string PortalName { get; set; } = "Unified Project Management Portal";
    public string HeaderTitle { get; set; } = "Project Management Portal";
    public string FooterText { get; set; } = "Built with React + .NET + MSSQL";
    public string PrimaryColor { get; set; } = "#2563eb";
    public string SecondaryColor { get; set; } = "#0f172a";
    public string HeroTitle { get; set; } = "Build and manage portals from one platform";
    public string HeroSubtitle { get; set; } = "Configure header, footer, menu links, announcements, and landing sections per customer.";
    public string AnnouncementText { get; set; } = "Welcome to your workspace.";
    public string SupportEmail { get; set; } = "support@portal.local";
    public string LogoUrl { get; set; } = "https://dummyimage.com/120x40/2563eb/ffffff&text=Portal";
    public bool ShowAnnouncements { get; set; } = true;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<PortalNavigationItem> NavigationItems { get; set; } = new List<PortalNavigationItem>();
    public ICollection<PortalContentSection> ContentSections { get; set; } = new List<PortalContentSection>();
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
}
