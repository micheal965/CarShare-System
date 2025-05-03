using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity.Roles;

namespace Youth_Innovation_System.Core.IServices.Identity
{
    public interface IRoleService
    {
        Task<ApiResponse> AddUserRoleAsync(AssignRoleDto assignRoleDto);
        Task<IList<string>> GetRolesAsync(string userId);
        Task<ApiResponse> UpdateUserRolesAsync(UpdateUserRoleDto updateUserRoleDto);
        Task<ApiResponse> DeleteUserRoleAsync(DeleteUserRoleDto deleteUserRoleDto);
    }
}
