namespace ProjectManagement.Api.Models;

public class PortalDesign
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerCode { get; set; } = "GLOBAL";
    public string HeaderTitle { get; set; } = "Project Management Portal";
    public string FooterText { get; set; } = "© 2026 Project Management";
    public string PrimaryColor { get; set; } = "#1d4ed8";
    public string AccentColor { get; set; } = "#0f172a";
    public bool ShowAnnouncements { get; set; } = true;
    public string? AnnouncementText { get; set; } = "Welcome to your workspace";
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
