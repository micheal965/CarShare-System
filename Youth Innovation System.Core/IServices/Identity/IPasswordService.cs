using Microsoft.AspNetCore.Identity;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity;

namespace Youth_Innovation_System.Core.IServices.Identity
{
    public interface IPasswordService
    {
        Task<ApiResponse> ChangePasswordAsync(string userId, ChangePasswordDto model);

        //after clicking the reset password link and enter inputdata
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string password);

    }
}
