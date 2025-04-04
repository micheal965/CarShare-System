using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity;

namespace Youth_Innovation_System.Service.IdentityServices
{
    public class PasswordService : IPasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordService(UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
        }
        public async Task<ApiResponse> ChangePasswordAsync(string userId, ChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "user not found");
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!checkPassword)
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Old Password is incorrect");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return new ApiExceptionResponse(StatusCodes.Status400BadRequest, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return new ApiResponse(StatusCodes.Status200OK, "Password Changed successfully");
        }

        // Forget Password
        //Not tested because it required integration with front
        /// <summary>
        /// after clicking the reset password link and enter inputdata
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid request." });
            return await _userManager.ResetPasswordAsync(user, token, password);
        }


    }
}
