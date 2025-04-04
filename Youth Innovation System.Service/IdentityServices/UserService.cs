using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IServices.Cloudinary;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Repository.Identity;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Identity;

namespace Youth_Innovation_System.Service.IdentityServices
{
    public class UserService : IUserService
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppIdentityDbContext appIdentityDbContext,
                            UserManager<ApplicationUser> userManager,
                            IMapper mapper,
                            ICloudinaryServices cloudinaryServices,
                            IHttpContextAccessor httpContextAccessor)
        {
            _appIdentityDbContext = appIdentityDbContext;
            _userManager = userManager;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not exists");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(UserRoles.Admin.ToString()))
                return new ApiResponse(StatusCodes.Status403Forbidden, "Deleting an Admin user is forbidden.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return new ApiResponse(StatusCodes.Status400BadRequest, "Something went wrong!");
            return new ApiResponse(StatusCodes.Status200OK, "User Deleted Successfully");
        }

        public async Task<IEnumerable<UserToReturnDto?>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserToReturnDto>>(users);
        }

        public async Task<IReadOnlyList<UserLoginHistory>> GetLoginHistoryAsync(string userId)
        {
            return await _appIdentityDbContext.userLoginHistories
                .Where(l => l.ApplicationUserId == userId)
                .OrderByDescending(l => l.LoginTime).ToListAsync();
        }

        public async Task<UserToReturnDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            return _mapper.Map<UserToReturnDto>(user);
        }

        public async Task SaveLoginAttemptAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

                // Check if the app is behind a proxy (e.g., Nginx, Cloudflare)
                if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();
                }

                ipAddress = ipAddress == "::1" ? "127.0.0.1" : ipAddress; // Convert ::1 to 127.0.0.1 if local
                await _appIdentityDbContext.userLoginHistories.AddAsync(new UserLoginHistory()
                {
                    ApplicationUserId = user.Id,
                    ipAddress = ipAddress,
                    LoginTime = DateTime.UtcNow,

                });
                await _appIdentityDbContext.SaveChangesAsync();
            }
        }

        public async Task<IdentityResult> AddOrUpdateProfilePictureAsync(string userId, IFormFile profilePicture)
        {
            var User = await _userManager.FindByIdAsync(userId);
            if (User == null) throw new Exception("User not found.");

            //Delete old profile picture if it exists
            if (!string.IsNullOrEmpty(User.pictureUrl))
            {
                await _cloudinaryServices.DeleteImageAsync(User.pictureUrl);
            }
            var imageUploadResult = await _cloudinaryServices.UploadImageAsync(profilePicture);

            User.pictureUrl = imageUploadResult.SecureUri.ToString();
            return await _userManager.UpdateAsync(User);
        }
        public async Task<bool> DeleteProfilePictureAsync(string userId)
        {
            var User = await _userManager.FindByIdAsync(userId);
            if (User == null) throw new Exception("User not found.");

            //Delete old profile picture if it exists
            bool DeleteImageResult = false;
            if (!string.IsNullOrEmpty(User.pictureUrl))
                DeleteImageResult = await _cloudinaryServices.DeleteImageAsync(User.pictureUrl);
            if (DeleteImageResult)
            {
                User.pictureUrl = null;
                await _userManager.UpdateAsync(User);
            }

            return DeleteImageResult;
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                _mapper.Map(userDto, user);

            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GetProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not Found");

            var result = await _cloudinaryServices.GetImageAsync(user.pictureUrl);
            return result;
        }

    }
}
