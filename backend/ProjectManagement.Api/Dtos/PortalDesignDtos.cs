namespace ProjectManagement.Api.Dtos;

public record PortalDesignResponse(
    Guid Id,
    string CustomerCode,
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string AccentColor,
    bool ShowAnnouncements,
    string? AnnouncementText,
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string AccentColor,
    bool ShowAnnouncements,
    string? AnnouncementText);
