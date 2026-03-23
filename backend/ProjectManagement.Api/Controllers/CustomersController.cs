using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Data;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "CanManageCustomers")]
public class CustomersController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CustomerSummaryResponse>>> GetCustomers()
    {
        var users = db.Users.AsNoTracking();
        var portals = db.PortalDesigns.AsNoTracking();

        var summaries = await users
            .GroupBy(user => user.CustomerCode)
            .Select(group => new CustomerSummaryResponse(
                group.Key,
                group.Count(),
                group.Count(user => user.Role == RoleNames.CustomerEmployee || user.Role == RoleNames.CustomerAdmin),
                group.Count(user => user.IsActive),
                portals.Where(portal => portal.CustomerCode == group.Key).Select(portal => (DateTime?)portal.UpdatedAtUtc).FirstOrDefault()))
            .OrderBy(item => item.CustomerCode)
            .ToListAsync();

        return Ok(summaries);
    }
}
