using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity;

namespace Youth_Innovation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;

        public UserController(IUserService userService, IPasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }
        [Authorize]
        [HttpGet("login-history/{userId}")]
        public async Task<IActionResult> GetLoginHistory(string userId)
        {
            var historyList = await _userService.GetLoginHistoryAsync(userId);
            if (!historyList.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "There is no login history"));
            return Ok(historyList);
        }
        [Authorize]
        [HttpPost("Change-Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _passwordService.ChangePasswordAsync(userId, changePasswordDto);
            return StatusCode(result.StatusCode, result);

        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpDelete("Delete-User/{UserId}")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var result = await _userService.DeleteUserAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any())
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No users found."));
            return Ok(users);
        }
        [Authorize]
        [HttpGet("Get-User/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "User not found!"));
            return Ok(user);
        }
        [Authorize]
        [HttpPost("Update-User")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            if (!result.Succeeded)
                return BadRequest(new ApiExceptionResponse(StatusCodes.Status400BadRequest, "Failed to update user", string.Join(",", result.Errors.Select(e => e.Description))));
            return Ok(new ApiResponse(StatusCodes.Status200OK, "User informations updated successfully"));
        }
        [Authorize]
        [HttpPost("AddOrUpdate-Profile-Picture")]
        public async Task<IActionResult> AddOrUpdateProfilePicture(IFormFile profilePicture)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var result = await _userService.AddOrUpdateProfilePictureAsync(userId, profilePicture);
                if (!result.Succeeded)
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Could not modify profile picture"));

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Profile picture modified successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }
        [Authorize]
        [HttpDelete("Delete-Profile-Picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var result = await _userService.DeleteProfilePictureAsync(userId);
                if (!result)
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Could not delete profile picture"));

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Profile picture deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }
        [HttpGet("Get-Profile-Picture/{userId}")]
        public async Task<IActionResult> GetProfilePicture(string userId)
        {
            try
            {
                return Ok(new { statusCode = StatusCodes.Status200OK, imageUrl = await _userService.GetProfilePictureAsync(userId) });
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
    }
}