using ASP_16._TaskFlow_Ownership.Common;
using ASP_16._TaskFlow_Ownership.DTOs.Auth_DTOs;
using ASP_16._TaskFlow_Ownership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_16._TaskFlow_Ownership.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "AdminOnly")]
public class UserRolesController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
    : ControllerBase {
    
    /// <summary>
    /// Retrieves all roles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserWithRolesDto>>>> GetAll()
    {
        var users = userManager
                            .Users
                            .OrderBy(u => u.Email)
                            .ToList();
        var usersWithRoles = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            usersWithRoles.Add(new UserWithRolesDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            });
        }
        return Ok(ApiResponse<IEnumerable<UserWithRolesDto>>.SuccessResponse(usersWithRoles, "List of users"));
    }

    /// <summary>
    /// Retrieves roles of specific user
    /// </summary>
    [HttpGet("{userId}/roles")]
    public async Task<ActionResult<ApiResponse<UserWithRolesDto>>> GetRoles(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return NotFound();

        var roles = await userManager.GetRolesAsync(user);

        var userWithRoles = new UserWithRolesDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
        return Ok(ApiResponse<UserWithRolesDto>.SuccessResponse(userWithRoles, "List of users"));
    }
    
    /// <summary>
    /// Assigns roles to specific user
    /// </summary>
    [HttpPost("{userId}/roles")]
    public async Task<ActionResult<ApiResponse<UserWithRolesDto>>> AssignRole(string userId, [FromBody] AssignRoleDto assignRole)
    {
        var roleName = assignRole.Role.Trim();

        if (string.IsNullOrEmpty(roleName))
            return BadRequest();

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return NotFound();

        if (!await roleManager.RoleExistsAsync(roleName))
            return BadRequest();

        if (await userManager.IsInRoleAsync(user, roleName))
            return BadRequest();

        var result = await userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded)
            return BadRequest();

        var roles = await userManager.GetRolesAsync(user);

        var userWithRoles = new UserWithRolesDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
        return Ok(ApiResponse<UserWithRolesDto>.SuccessResponse(userWithRoles, "User with role"));
    }

    /// <summary>
    /// Deletes specific role of specific user
    /// </summary>
    [HttpDelete("{userId}/roles/{roleName}")]
    public async Task<ActionResult<ApiResponse<UserWithRolesDto>>> DeleteRole(string userId, string roleName)
    {
        roleName = roleName.Trim();

        if (string.IsNullOrEmpty(roleName))
            return BadRequest();

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return NotFound();

        if (!await roleManager.RoleExistsAsync(roleName))
            return BadRequest();

        if (!await userManager.IsInRoleAsync(user, roleName))
            return BadRequest();

        var result = await userManager.RemoveFromRoleAsync(user, roleName);

        if (!result.Succeeded)
            return BadRequest();

        var roles = await userManager.GetRolesAsync(user);

        var userWithRoles = new UserWithRolesDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
        return Ok(ApiResponse<UserWithRolesDto>.SuccessResponse(userWithRoles, "User with role"));
    }

}