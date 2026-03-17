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
    IReadOnlyList<PortalPageResponse> Pages,
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string AccentColor,
    bool ShowAnnouncements,
    string? AnnouncementText,
    IReadOnlyList<PortalPageRequest>? Pages);

public record PortalPageResponse(
    string Id,
    string Name,
    string Slug,
    string HeroTitle,
    string HeroText,
    string SectionTitle,
    string SectionText,
    string[] BulletPoints,
    string CtaLabel,
    string CtaLink,
    string ThemePreset);

public record PortalPageRequest(
    string? Id,
    string? Name,
    string? Slug,
    string? HeroTitle,
    string? HeroText,
    string? SectionTitle,
    string? SectionText,
    IReadOnlyList<string>? BulletPoints,
    string? CtaLabel,
    string? CtaLink,
    string? ThemePreset);
