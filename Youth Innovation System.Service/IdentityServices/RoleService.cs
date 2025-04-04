using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity.Roles;

namespace Youth_Innovation_System.Service.IdentityServices
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<ApiResponse> UpdateUserRolesAsync(UpdateUserRoleDto updateUserRoleDto)
        {
            var user = await _userManager.FindByIdAsync(updateUserRoleDto.userId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not found!");

            foreach (var role in updateUserRoleDto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    return new ApiResponse(StatusCodes.Status404NotFound, $"Role {role} does not exist.");
            }

            var existingRoles = await _userManager.GetRolesAsync(user);

            //Preventing deleting the last admin in db
            if (existingRoles.Contains(UserRoles.Admin.ToString()) && !updateUserRoleDto.Roles.Contains(UserRoles.Admin.ToString()))
            {
                var totalAdmins = await _userManager.GetUsersInRoleAsync(UserRoles.Admin.ToString());
                if (totalAdmins.Count == 1)
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Cannot Remove the last admin.");
            }
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
            var result = await _userManager.AddToRolesAsync(user, updateUserRoleDto.Roles);

            if (!result.Succeeded)
                return new ApiExceptionResponse(StatusCodes.Status400BadRequest, "Failed to update roles.", string.Join(",", result.Errors.Select(e => e.Description)));
            return new ApiResponse(StatusCodes.Status200OK, "User roles updated successfully");



        }
        public async Task<ApiResponse> AddUserRoleAsync(AssignRoleDto assignRoleDto)
        {
            //Check user exist
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not found");
            //Check if role exists
            if (!await _roleManager.RoleExistsAsync(assignRoleDto.role))
                return new ApiResponse(StatusCodes.Status404NotFound, "Role not found");

            var result = await _userManager.AddToRoleAsync(user, assignRoleDto.role);
            if (!result.Succeeded)
            {
                return new ApiExceptionResponse(StatusCodes.Status400BadRequest, "Failed to assign role", string.Join(',', result.Errors.Select(e => e.Description)));
            }
            return new ApiResponse(StatusCodes.Status200OK, $"Role {assignRoleDto.role} assigned to User {user.firstName} successfully");
        }

        public async Task<ApiResponse> DeleteUserRoleAsync(DeleteUserRoleDto deleteUserRoleDto)
        {
            var user = await _userManager.FindByIdAsync(deleteUserRoleDto.UserId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not found.");

            if (!await _roleManager.RoleExistsAsync(deleteUserRoleDto.Role))
                return new ApiResponse(StatusCodes.Status404NotFound, "Role does not exist");

            if (!await _userManager.IsInRoleAsync(user, deleteUserRoleDto.Role))
                return new ApiResponse(StatusCodes.Status400BadRequest, "User does not have this role!");
            //Preventing deleting the last admin
            if (deleteUserRoleDto.Role == UserRoles.Admin.ToString())
            {
                var existingAdmins = await _userManager.GetUsersInRoleAsync(UserRoles.Admin.ToString());
                if (existingAdmins.Count == 1)
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Cannot remove the last Admin");
            }
            var result = await _userManager.RemoveFromRoleAsync(user, deleteUserRoleDto.Role);
            if (!result.Succeeded)
                return new ApiExceptionResponse(StatusCodes.Status400BadRequest, "Failed to remove role");

            return new ApiResponse(StatusCodes.Status200OK, $"Role {deleteUserRoleDto.Role} removed from User {user.firstName} successfully");
        }
    }
}

