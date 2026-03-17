using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Dtos;
using ProjectManagement.Api.Services;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "GlobalAdminOnly")]
public class AdminController(IAdminManagementService adminManagementService) : ControllerBase
{
    [HttpGet("companies")]
    public async Task<ActionResult<IReadOnlyList<CompanyListItemResponse>>> GetCompanies([FromQuery] string? search, [FromQuery] bool? isLoginAllowed)
    {
        return Ok(await adminManagementService.GetCompaniesAsync(search, isLoginAllowed));
    }

    [HttpGet("companies/{companyId:guid}")]
    public async Task<ActionResult<CompanyDetailsResponse>> GetCompany(Guid companyId)
    {
        try
        {
            return Ok(await adminManagementService.GetCompanyAsync(companyId));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("companies/{companyId:guid}")]
    public async Task<ActionResult<CompanyDetailsResponse>> UpdateCompany(Guid companyId, UpdateCompanyRequest request)
    {
        try
        {
            return Ok(await adminManagementService.UpdateCompanyAsync(companyId, request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<AdminUserListItemResponse>>> GetUsers([FromQuery] string? search, [FromQuery] string? companyCode, [FromQuery] bool? isLoginAllowed)
    {
        return Ok(await adminManagementService.GetUsersAsync(search, companyCode, isLoginAllowed));
    }

    [HttpPut("users/{userId:guid}/login-status")]
    public async Task<ActionResult<AdminUserListItemResponse>> UpdateUserLoginStatus(Guid userId, UpdateUserLoginStatusRequest request)
    {
        try
        {
            return Ok(await adminManagementService.UpdateUserLoginStatusAsync(GetCurrentUserId(), userId, request));
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
