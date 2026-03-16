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
    [HttpGet("me")]
    public async Task<ActionResult<PortalDesignResponse>> GetMyPortalDesign()
    {
        var customerCode = User.FindFirstValue("customer_code") ?? "GLOBAL";

        var design = await db.PortalDesigns
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.CustomerCode == customerCode)
            ?? await db.PortalDesigns.AsNoTracking().FirstAsync(item => item.CustomerCode == "GLOBAL");

        return Ok(ToResponse(design));
    }

    [HttpPut("{customerCode}")]
    [Authorize(Policy = "CanManagePortal")]
    public async Task<ActionResult<PortalDesignResponse>> UpsertPortalDesign(string customerCode, UpsertPortalDesignRequest request)
    {
        customerCode = customerCode.Trim().ToUpperInvariant();
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userCustomerCode = User.FindFirstValue("customer_code") ?? "GLOBAL";

        if (userRole == RoleNames.CustomerEmployee && userCustomerCode != customerCode)
        {
            return Forbid();
        }

        var design = await db.PortalDesigns.FirstOrDefaultAsync(item => item.CustomerCode == customerCode);
        if (design is null)
        {
            design = new PortalDesign { CustomerCode = customerCode };
            db.PortalDesigns.Add(design);
        }

        design.HeaderTitle = request.HeaderTitle;
        design.FooterText = request.FooterText;
        design.PrimaryColor = request.PrimaryColor;
        design.AccentColor = request.AccentColor;
        design.ShowAnnouncements = request.ShowAnnouncements;
        design.AnnouncementText = request.AnnouncementText;
        design.UpdatedAtUtc = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Ok(ToResponse(design));
    }

    private static PortalDesignResponse ToResponse(PortalDesign design) =>
        new(
            design.Id,
            design.CustomerCode,
            design.HeaderTitle,
            design.FooterText,
            design.PrimaryColor,
            design.AccentColor,
            design.ShowAnnouncements,
            design.AnnouncementText,
            design.UpdatedAtUtc);
}
