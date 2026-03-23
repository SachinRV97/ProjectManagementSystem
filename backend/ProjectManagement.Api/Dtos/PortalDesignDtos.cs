namespace ProjectManagement.Api.Dtos;

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
public record PortalDesignResponse(
    Guid Id,
    string CustomerCode,
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    string SiteName,
    string SiteSlug,
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string AccentColor,
    bool ShowAnnouncements,
    string? AnnouncementText,
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    IReadOnlyList<PortalPageResponse> Pages,
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
    string SiteName,
=======
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
>>>>>>> theirs
=======
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
>>>>>>> theirs
=======
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
>>>>>>> theirs
=======
    DateTime UpdatedAtUtc);

public record UpsertPortalDesignRequest(
>>>>>>> theirs
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string AccentColor,
    bool ShowAnnouncements,
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
    string? AnnouncementText);
>>>>>>> theirs
=======
    string? AnnouncementText);
>>>>>>> theirs
=======
    string? AnnouncementText);
>>>>>>> theirs
=======
    string? AnnouncementText);
>>>>>>> theirs
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
public record PortalNavigationItemDto(Guid? Id, string Label, string Href, int SortOrder, bool OpenInNewTab);

public record PortalContentSectionDto(Guid? Id, string SectionKey, string Title, string Body, int SortOrder);

public record PortalDesignResponse(
    Guid Id,
    string CustomerCode,
    string PortalName,
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string SecondaryColor,
    string HeroTitle,
    string HeroSubtitle,
    string AnnouncementText,
    string SupportEmail,
    string LogoUrl,
    bool ShowAnnouncements,
    DateTime UpdatedAtUtc,
    IReadOnlyCollection<PortalNavigationItemDto> NavigationItems,
    IReadOnlyCollection<PortalContentSectionDto> ContentSections);

public record UpsertPortalDesignRequest(
    string PortalName,
    string HeaderTitle,
    string FooterText,
    string PrimaryColor,
    string SecondaryColor,
    string HeroTitle,
    string HeroSubtitle,
    string AnnouncementText,
    string SupportEmail,
    string LogoUrl,
    bool ShowAnnouncements,
    IReadOnlyCollection<PortalNavigationItemDto> NavigationItems,
    IReadOnlyCollection<PortalContentSectionDto> ContentSections);
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
