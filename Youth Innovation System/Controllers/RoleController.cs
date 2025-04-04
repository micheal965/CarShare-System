using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity.Roles;

namespace Youth_Innovation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [Authorize]
        [HttpGet("Get-User-Roles/{UserId}")]
        public async Task<IActionResult> GetRolesOfUser(string UserId)
        {
            var result = await _roleService.GetRolesAsync(UserId);
            if (!result.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "User not found or User has no roles."));
            return Ok(new { roles = result });
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpPut("Update-Role")]
        public async Task<IActionResult> UpdateRolesOfUser(UpdateUserRoleDto updateUserRoleDto)
        {
            if (string.IsNullOrWhiteSpace(updateUserRoleDto.userId) || updateUserRoleDto.Roles == null || updateUserRoleDto.Roles.Count == 0)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "User ID and roles are required."));
            var result = await _roleService.UpdateUserRolesAsync(updateUserRoleDto);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpPost("Add-User-Role")]
        public async Task<IActionResult> AddRoleToUser(AssignRoleDto assignRoleDto)
        {
            var result = await _roleService.AddUserRoleAsync(assignRoleDto);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpDelete("Delete-User-Role")]
        public async Task<IActionResult> DeleteRoleFromUser(DeleteUserRoleDto deleteUserRoleDto)
        {
            var result = await _roleService.DeleteUserRoleAsync(deleteUserRoleDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
