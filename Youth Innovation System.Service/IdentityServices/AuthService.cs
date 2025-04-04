using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IServices.Cloudinary;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.DTOs.Identity;
using Youth_Innovation_System.Shared.DTOs.Identity;
using Youth_Innovation_System.Shared.Exceptions;

namespace Youth_Innovation_System.Service.IdentityServices
{
    public class AuthService : IAuthService
    {
        private static readonly HashSet<string> BlacklistedTokens = new();

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(IConfiguration configuration,
                           IUserService userService,
                           IHttpContextAccessor httpContextAccessor,
                           ICloudinaryServices cloudinaryServices,
                           UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryServices = cloudinaryServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            //Ensuring user exist by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid login attempt!");

            if (user.status != UserStatus.accepted.ToString())
                throw new UnauthorizedAccessException("Your account has not been approved yet or has been rejected.");

            //checking password
            //assume no lockout
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.IsPersistent, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid login attempt!");

            //track ipAddress in userloginhistory table
            await _userService.SaveLoginAttemptAsync(loginDto.Email);
            //returning Response
            var roles = await _userManager.GetRolesAsync(user);
            //Check for refreshToken
            var RefreshTokenObj = new RefreshToken();
            if (user.refreshTokens.Any(t => t.isActive))
            {
                //if there is an active refreshtoken
                RefreshTokenObj = user.refreshTokens.FirstOrDefault(t => t.isActive);
            }
            else
            {
                //if there is no activeRefreshtoken for that user so generate new one
                RefreshTokenObj = GenerateRefreshTokenObject();
                user.refreshTokens.Add(RefreshTokenObj);
                await _userManager.UpdateAsync(user);

            }

            //set refresh token in the cookies 
            if (!string.IsNullOrEmpty(RefreshTokenObj.token))
                AppendRefreshTokenInCookies(RefreshTokenObj.token, RefreshTokenObj.expiryDate);

            return new LoginResponseDto()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Token = await CreateJwtWebTokenAsync(user),
                Roles = roles.ToList(),
                refreshToken = RefreshTokenObj.token,
                refreshTokenExpiration = RefreshTokenObj.expiryDate,
            };
        }
        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {

            ImageUploadResult imageUploadResult = null;
            if (registerDto.ProfilePicture != null)
            {
                try
                {
                    imageUploadResult = await _cloudinaryServices.UploadImageAsync(registerDto.ProfilePicture);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Image upload failed: {ex.Message}");
                }
            }

            var user = new ApplicationUser()
            {
                UserName = registerDto.Email.Split("@")[0],
                Email = registerDto.Email,
                firstName = registerDto.FirstName,
                lastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                pictureUrl = imageUploadResult?.SecureUri.ToString(),
            };
            //ensuring email doesn't exist before
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                throw new Exception($"Email {registerDto.Email} is already taken!");

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            //Adding role
            string role = string.Empty;
            switch (registerDto.role)
            {
                case 0:
                    role = UserRoles.CarOwner.ToString();
                    break;
                case 1:
                    role = UserRoles.Renter.ToString();
                    break;
                default:
                    throw new ArgumentException("Invalid role value");
            }
            var addRoleresult = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            if (!addRoleresult.Succeeded)
                throw new Exception(string.Join(", ", addRoleresult.Errors.Select(e => e.Description)));
            return result;
        }
        public async Task<string> CreateJwtWebTokenAsync(ApplicationUser user)
        {
            //Authentication Claims
            var authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            //RoleClaims 
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles != null)
            {
                foreach (var role in userRoles)
                    authclaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: authclaims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"])),
                signingCredentials: cred
                );

            //write token and return
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            await Task.Delay(100); // Simulate a delay
            return BlacklistedTokens.Contains(token);
        }

        // Simulate async I/O operation to add token to blacklist
        public async Task BlacklistTokenAsync(string token)
        {
            await Task.Delay(100); // Simulate a delay
            BlacklistedTokens.Add(token);
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.refreshTokens.Any(rf => rf.token == refreshToken));
            if (user != null)
            {
                var refreshtokenObj = user.refreshTokens.Single(rf => rf.token == refreshToken);
                refreshtokenObj.revokedOn = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
                DeleteRefreshTokenFromCookies();
            }
        }
        private void DeleteRefreshTokenFromCookies()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
        }
        private RefreshToken GenerateRefreshTokenObject()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return new RefreshToken()
            {
                token = Convert.ToBase64String(randomBytes),
                createdOn = DateTime.UtcNow,
                expiryDate = DateTime.UtcNow.AddDays(7),
            };
        }
        private void AppendRefreshTokenInCookies(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = expires,
                SameSite = SameSiteMode.Strict,
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        public async Task<RotateRefreshTokenResponseDto> RotateRefreshTokenAsync(string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.refreshTokens.Any(t => t.token == token));
            if (user == null)
                return new RotateRefreshTokenResponseDto() { Message = "Invalid Token" };

            var refreshToken = user.refreshTokens.Single(rt => rt.token == token);
            if (!refreshToken.isActive)
                return new RotateRefreshTokenResponseDto() { Message = "InActive Token" };
            //revoke that token and generate new one
            refreshToken.revokedOn = DateTime.UtcNow;

            var newRefreshTokenObj = GenerateRefreshTokenObject();
            user.refreshTokens.Add(newRefreshTokenObj);
            await _userManager.UpdateAsync(user);
            //Delete old refreshtoken and save the new refresh token into cookies=>(Append)
            AppendRefreshTokenInCookies(newRefreshTokenObj.token, newRefreshTokenObj.expiryDate);

            //get roles from db for that user
            var roles = await _userManager.GetRolesAsync(user);
            return new RotateRefreshTokenResponseDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                //Generate new jwt token
                Token = await CreateJwtWebTokenAsync(user),
                Roles = roles.ToList(),
                refreshToken = newRefreshTokenObj.token,
                refreshTokenExpiration = newRefreshTokenObj.expiryDate,
                IsAuthenticated = true,
                Message = "Token Rotated successfully!"
            };
        }
        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.refreshTokens.Any(rf => rf.token == token));
            if (user == null) return false;
            var refreshToken = user.refreshTokens.Single(rf => rf.token == token);
            if (!refreshToken.isActive) return false;

            refreshToken.revokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task ManageCarOwnerAccount(string userId, bool IsApproved)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains(UserRoles.CarOwner.ToString()))
                throw new UnauthorizedAccessException("Only Car Owner accounts can be managed.");

            if (user.status == UserStatus.accepted.ToString())
                throw new InvalidOperationException("Accepted accounts cannot be modified.");

            user.status = IsApproved ? UserStatus.accepted.ToString() : UserStatus.rejected.ToString();
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to update user status.");
        }
    }
}
