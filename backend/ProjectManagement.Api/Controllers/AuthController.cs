using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Services;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("roles")]
    public async Task<ActionResult<IReadOnlyList<RoleResponse>>> GetRoles()
    {
        return Ok(await authService.GetAvailableRolesAsync());
    }

    [Authorize]
    [HttpGet("assignable-roles")]
    public async Task<ActionResult<IReadOnlyList<RoleResponse>>> GetAssignableRoles()
    {
        try
        {
            return Ok(await authService.GetAssignableRolesAsync(GetCurrentUserId()));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("register-company")]
    public async Task<ActionResult<AuthResponse>> RegisterCompany(RegisterCompanyRequest request)
    {
        try
        {
            return Ok(await authService.RegisterCompanyAsync(request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [Authorize]
    [HttpPost("register-user")]
    public async Task<ActionResult<RegisteredUserResponse>> RegisterUser(RegisterUserRequest request)
    {
        try
        {
            return Ok(await authService.RegisterUserAsync(GetCurrentUserId(), request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            return Ok(await authService.LoginAsync(request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(value, out var userId))
        {
            throw new InvalidOperationException("Current user is invalid.");
        }

        return userId;
    }
}
