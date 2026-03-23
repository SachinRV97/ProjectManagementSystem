<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Services;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            return Ok(await authService.RegisterAsync(request));
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

<<<<<<< ours
    [AllowAnonymous]
    [HttpPost("register-company")]
    public async Task<ActionResult<AuthResponse>> RegisterCompany(RegisterCompanyRequest request)
    {
        try
        {
            return Ok(await authService.RegisterCompanyAsync(request));
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
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
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
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
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
<<<<<<< ours

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
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs
}
