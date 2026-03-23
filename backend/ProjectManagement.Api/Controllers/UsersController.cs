using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Data;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Services;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "CanManageUsers")]
public class UsersController(AppDbContext db, IAuthService authService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UserListItemResponse>>> GetUsers()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        var customerCode = User.FindFirstValue("customer_code") ?? "GLOBAL";

        var query = db.Users.AsNoTracking();
        if (role == RoleNames.CustomerAdmin)
        {
            query = query.Where(user => user.CustomerCode == customerCode);
        }

        var users = await query
            .OrderBy(user => user.CustomerCode)
            .ThenBy(user => user.Name)
            .Select(user => new UserListItemResponse(user.Id, user.Name, user.Email, user.Role, user.CustomerCode, user.IsActive, user.CreatedAtUtc))
            .ToListAsync();

        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserListItemResponse>> CreateUser(CreateManagedUserRequest request)
    {
        try
        {
            var actor = await GetActorAsync();
            var user = await authService.CreateManagedUserAsync(request, actor);
            return Ok(new UserListItemResponse(user.Id, user.Name, user.Email, user.Role, user.CustomerCode, user.IsActive, user.CreatedAtUtc));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    private async Task<ApplicationUser> GetActorAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        return await db.Users.FirstAsync(user => user.Email == email || user.Id.ToString() == userId);
    }
}
