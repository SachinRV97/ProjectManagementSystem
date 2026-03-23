<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Data;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PortalDesignController(AppDbContext db) : ControllerBase
{
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
    private static readonly JsonSerializerOptions PageJsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly HashSet<string> AllowedThemePresets = new(StringComparer.OrdinalIgnoreCase)
    {
        "aurora",
        "ocean",
        "sunrise",
        "midnight"
    };

    [HttpGet("me")]
    public async Task<ActionResult<PortalDesignResponse>> GetMyPortalDesign()
    {
        var companyCode = User.FindFirstValue(CustomClaimTypes.CompanyCode) ?? "GLOBAL";
=======
    [HttpGet("me")]
    public async Task<ActionResult<PortalDesignResponse>> GetMyPortalDesign()
    {
>>>>>>> theirs
=======
    [HttpGet("me")]
    public async Task<ActionResult<PortalDesignResponse>> GetMyPortalDesign()
    {
>>>>>>> theirs
=======
    [HttpGet("me")]
    public async Task<ActionResult<PortalDesignResponse>> GetMyPortalDesign()
    {
>>>>>>> theirs
=======
    [HttpGet("me")]
    public async Task<ActionResult<PortalDesignResponse>> GetMyPortalDesign()
    {
>>>>>>> theirs
        var customerCode = User.FindFirstValue("customer_code") ?? "GLOBAL";

        var design = await db.PortalDesigns
            .AsNoTracking()
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
            .FirstOrDefaultAsync(item =>
                item.CompanyCode == companyCode &&
                item.CustomerCode == customerCode)
            ?? await db.PortalDesigns.AsNoTracking().FirstOrDefaultAsync(item =>
                item.CompanyCode == companyCode &&
                item.CustomerCode == "GLOBAL")
            ?? await db.PortalDesigns.AsNoTracking().FirstAsync(item =>
                item.CompanyCode == "GLOBAL" &&
                item.CustomerCode == "GLOBAL");

        return Ok(ToResponse(design));
    }

    [AllowAnonymous]
    [HttpGet("site/{siteName}")]
    public async Task<ActionResult<PortalDesignResponse>> GetPublicPortalDesign(string siteName)
    {
        var normalizedSiteSlug = NormalizeRouteSlug(siteName, "portal");
        var designs = await db.PortalDesigns
            .AsNoTracking()
            .ToListAsync();

        var design = designs.FirstOrDefault(item => MatchesSite(item, normalizedSiteSlug));
        if (design is null)
        {
            return NotFound(new { message = "Portal site was not found." });
        }
=======
            .FirstOrDefaultAsync(item => item.CustomerCode == customerCode)
            ?? await db.PortalDesigns.AsNoTracking().FirstAsync(item => item.CustomerCode == "GLOBAL");
>>>>>>> theirs
=======
            .FirstOrDefaultAsync(item => item.CustomerCode == customerCode)
            ?? await db.PortalDesigns.AsNoTracking().FirstAsync(item => item.CustomerCode == "GLOBAL");
>>>>>>> theirs
=======
            .FirstOrDefaultAsync(item => item.CustomerCode == customerCode)
            ?? await db.PortalDesigns.AsNoTracking().FirstAsync(item => item.CustomerCode == "GLOBAL");
>>>>>>> theirs
=======
            .FirstOrDefaultAsync(item => item.CustomerCode == customerCode)
            ?? await db.PortalDesigns.AsNoTracking().FirstAsync(item => item.CustomerCode == "GLOBAL");
>>>>>>> theirs

        return Ok(ToResponse(design));
    }

    [HttpPut("{customerCode}")]
    [Authorize(Policy = "CanManagePortal")]
    public async Task<ActionResult<PortalDesignResponse>> UpsertPortalDesign(string customerCode, UpsertPortalDesignRequest request)
    {
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
        var companyCode = User.FindFirstValue(CustomClaimTypes.CompanyCode) ?? "GLOBAL";
        customerCode = customerCode.Trim().ToUpperInvariant();
        var userCustomerCode = User.FindFirstValue("customer_code") ?? "GLOBAL";
        var isOwnCustomerScope = User.HasClaim(CustomClaimTypes.PortalScope, PortalScopeValues.OwnCustomer);

        if (isOwnCustomerScope && userCustomerCode != customerCode)
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        customerCode = customerCode.Trim().ToUpperInvariant();
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userCustomerCode = User.FindFirstValue("customer_code") ?? "GLOBAL";

        if (userRole == RoleNames.CustomerEmployee && userCustomerCode != customerCode)
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        {
            return Forbid();
        }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
        var design = await db.PortalDesigns.FirstOrDefaultAsync(item =>
            item.CompanyCode == companyCode &&
            item.CustomerCode == customerCode);
        if (design is null)
        {
            design = new PortalDesign
            {
                CompanyCode = companyCode,
                CustomerCode = customerCode,
                SiteName = NormalizeOrFallback(request.SiteName, request.HeaderTitle)
            };
            db.PortalDesigns.Add(design);
        }

        design.SiteName = NormalizeOrFallback(request.SiteName, request.HeaderTitle);
        design.SiteSlug = await GenerateUniqueSiteSlugAsync(design.SiteName, design.Id);
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        var design = await db.PortalDesigns.FirstOrDefaultAsync(item => item.CustomerCode == customerCode);
        if (design is null)
        {
            design = new PortalDesign { CustomerCode = customerCode };
            db.PortalDesigns.Add(design);
        }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        design.HeaderTitle = request.HeaderTitle;
        design.FooterText = request.FooterText;
        design.PrimaryColor = request.PrimaryColor;
        design.AccentColor = request.AccentColor;
        design.ShowAnnouncements = request.ShowAnnouncements;
        design.AnnouncementText = request.AnnouncementText;
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
        design.PageConfigurationsJson = SerializePages(
            NormalizePages(
                request.Pages,
                request.HeaderTitle,
                request.AnnouncementText));
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        design.UpdatedAtUtc = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Ok(ToResponse(design));
    }

    private static PortalDesignResponse ToResponse(PortalDesign design) =>
        new(
            design.Id,
            design.CustomerCode,
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
            ResolveSiteName(design),
            ResolveSiteSlug(design),
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
            design.HeaderTitle,
            design.FooterText,
            design.PrimaryColor,
            design.AccentColor,
            design.ShowAnnouncements,
            design.AnnouncementText,
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
            ReadPages(design.PageConfigurationsJson, design.HeaderTitle, design.AnnouncementText),
            design.UpdatedAtUtc);

    private static IReadOnlyList<PortalPageResponse> ReadPages(string? pageConfigurationsJson, string headerTitle, string? announcementText)
    {
        if (!string.IsNullOrWhiteSpace(pageConfigurationsJson))
        {
            try
            {
                var pages = JsonSerializer.Deserialize<List<PortalPageResponse>>(pageConfigurationsJson, PageJsonOptions);
                if (pages is { Count: > 0 })
                {
                    return pages;
                }
            }
            catch (JsonException)
            {
            }
        }

        return BuildDefaultPages(headerTitle, announcementText);
    }

    private static string SerializePages(IReadOnlyList<PortalPageResponse> pages) =>
        JsonSerializer.Serialize(pages, PageJsonOptions);

    private async Task<string> GenerateUniqueSiteSlugAsync(string siteName, Guid designId)
    {
        var baseSlug = NormalizeRouteSlug(siteName, $"site-{designId:N}");
        var candidate = baseSlug;
        var suffix = 2;

        while (await db.PortalDesigns.AnyAsync(item =>
            item.Id != designId &&
            item.SiteSlug == candidate))
        {
            candidate = $"{baseSlug}-{suffix++}";
        }

        return candidate;
    }

    private static IReadOnlyList<PortalPageResponse> NormalizePages(
        IReadOnlyList<PortalPageRequest>? pages,
        string headerTitle,
        string? announcementText)
    {
        if (pages is null || pages.Count == 0)
        {
            return BuildDefaultPages(headerTitle, announcementText);
        }

        var normalizedPages = new List<PortalPageResponse>();
        var usedSlugs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var index = 0; index < pages.Count; index++)
        {
            var page = pages[index];
            var pageNumber = index + 1;
            var name = NormalizeOrFallback(page.Name, $"Page {pageNumber}");
            var slug = EnsureUniqueSlug(
                NormalizeSlug(page.Slug, name, pageNumber),
                usedSlugs);

            normalizedPages.Add(new PortalPageResponse(
                NormalizeOrFallback(page.Id, Guid.NewGuid().ToString("N")),
                name,
                slug,
                NormalizeOrFallback(page.HeroTitle, name),
                NormalizeOrFallback(page.HeroText, announcementText ?? "Design this page with your own message and content blocks."),
                NormalizeOrFallback(page.SectionTitle, "Highlights"),
                NormalizeOrFallback(page.SectionText, "Use the page builder to shape sections, messaging, and calls to action."),
                NormalizeBulletPoints(page.BulletPoints),
                NormalizeOrFallback(page.CtaLabel, "Learn More"),
                NormalizeOrFallback(page.CtaLink, "#"),
                NormalizeThemePreset(page.ThemePreset)));
        }

        return normalizedPages;
    }

    private static IReadOnlyList<PortalPageResponse> BuildDefaultPages(string headerTitle, string? announcementText) =>
    [
        new PortalPageResponse(
            "home",
            "Home",
            "home",
            headerTitle,
            NormalizeOrFallback(announcementText, "Design branded pages for employees and customers from the portal builder."),
            "Why This Portal",
            "Create flexible landing pages, service pages, and customer-facing layouts from one admin experience.",
            ["Custom hero content", "Reusable highlight blocks", "Editable call to action"],
            "Get Started",
            "#",
            "aurora")
    ];

    private static string[] NormalizeBulletPoints(IReadOnlyList<string>? bulletPoints)
    {
        var normalized = (bulletPoints ?? [])
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Select(item => item.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(6)
            .ToArray();

        return normalized.Length > 0
            ? normalized
            : ["Customizable content block", "Flexible preview layout", "Editable page call to action"];
    }

    private static string NormalizeThemePreset(string? value)
    {
        var normalized = NormalizeOrFallback(value, "aurora").ToLowerInvariant();
        return AllowedThemePresets.Contains(normalized)
            ? normalized
            : "aurora";
    }

    private static bool MatchesSite(PortalDesign design, string normalizedSiteSlug)
    {
        var currentSiteSlug = ResolveSiteSlug(design);
        if (string.Equals(currentSiteSlug, normalizedSiteSlug, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var currentSiteNameSlug = NormalizeRouteSlug(ResolveSiteName(design), ResolveSiteName(design));
        if (string.Equals(currentSiteNameSlug, normalizedSiteSlug, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var headerSlug = NormalizeRouteSlug(design.HeaderTitle, design.HeaderTitle);
        return string.Equals(headerSlug, normalizedSiteSlug, StringComparison.OrdinalIgnoreCase);
    }

    private static string ResolveSiteName(PortalDesign design) =>
        NormalizeOrFallback(design.SiteName, design.HeaderTitle);

    private static string ResolveSiteSlug(PortalDesign design) =>
        NormalizeRouteSlug(design.SiteSlug, ResolveSiteName(design));

    private static string NormalizeSlug(string? requestedSlug, string fallbackName, int pageNumber)
    {
        var source = NormalizeOrFallback(requestedSlug, fallbackName);
        var builder = new StringBuilder();
        var lastWasDash = false;

        foreach (var character in source.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                lastWasDash = false;
            }
            else if (!lastWasDash)
            {
                builder.Append('-');
                lastWasDash = true;
            }
        }

        var normalized = Regex.Replace(builder.ToString().Trim('-'), "-{2,}", "-");
        return string.IsNullOrWhiteSpace(normalized)
            ? $"page-{pageNumber}"
            : normalized;
    }

    private static string NormalizeRouteSlug(string? requestedValue, string fallbackValue)
    {
        var source = NormalizeOrFallback(requestedValue, fallbackValue);
        var builder = new StringBuilder();
        var lastWasDash = false;

        foreach (var character in source.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                lastWasDash = false;
            }
            else if (!lastWasDash)
            {
                builder.Append('-');
                lastWasDash = true;
            }
        }

        var normalized = Regex.Replace(builder.ToString().Trim('-'), "-{2,}", "-");
        return string.IsNullOrWhiteSpace(normalized)
            ? "portal"
            : normalized;
    }

    private static string EnsureUniqueSlug(string slug, ISet<string> usedSlugs)
    {
        var candidate = slug;
        var suffix = 2;
        while (!usedSlugs.Add(candidate))
        {
            candidate = $"{slug}-{suffix++}";
        }

        return candidate;
    }

    private static string NormalizeOrFallback(string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
=======
            design.UpdatedAtUtc);
>>>>>>> theirs
=======
            design.UpdatedAtUtc);
>>>>>>> theirs
=======
            design.UpdatedAtUtc);
>>>>>>> theirs
=======
            design.UpdatedAtUtc);
>>>>>>> theirs
}
