using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IServices.Email;
using Youth_Innovation_System.Core.IServices.Identity;
using Youth_Innovation_System.Shared.ApiResponses;

namespace Youth_Innovation_System.Service.IdentityServices
{
    public class UserVerificationService : IUserVerificationService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserVerificationService(IConfiguration configuration,
                                        IEmailService emailService,
                                        IUrlHelperFactory urlHelperFactory,
                                        IActionContextAccessor actionContextAccessor,
                                        IHttpContextAccessor httpContextAccessor,
                                      UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _emailService = emailService;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }
        //send email contains verificationlink
        public async Task RequestVerificationEmailAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedEmailToken = Encoding.UTF8.GetBytes(token);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
                var link = urlHelper.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = validEmailToken }, _httpContextAccessor.HttpContext.Request.Scheme);

                string emailSubject = "Email Confirmation";
                string htmlBody = $@"
                        <html>

                        <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f3f4f6;'>
                            <div
                                style='max-width: 600px; margin: 30px auto; background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
                                <div style='background-color: #4CAF50; padding: 20px; text-align: center;'>
                                    <h1 style='color: white; margin: 0; font-size: 24px;'>Welcome to Youth Innovation System</h1>
                                </div>
                                <div style='padding: 20px; color: #333333;'>
                                    <h2 style='margin-top: 0;'>Hello, {user.firstName}!</h2>
                                    <p style='font-size: 16px; line-height: 1.6;'>
                                        Thank you for signing up with <strong>Youth Innovation System</strong>. We’re thrilled to have you
                                        onboard!
                                    </p>
                                    <p style='font-size: 16px; line-height: 1.6;'>
                                        To complete your registration, please verify your email address by clicking the button below:
                                    </p>
                                    <div style='text-align: center; margin: 30px 0;'>
                                        <a href='{link}'
                                            style='background-color: #4CAF50; color: white; padding: 12px 30px; font-size: 16px; font-weight: bold; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                            Verify Email
                                        </a>
                                    </div>

                                </div>
                                <div style='background-color: #f9f9f9; padding: 15px; text-align: center; font-size: 12px; color: #999999;'>
                                    <p style='margin: 5px 0 0;'>Need help? <a href='mailto:michealghobriall@gmail.com'
                                            style='color: #4CAF50;'>Contact
                                            Us</a></p>
                                </div>
                            </div>
                        </body>

                        </html>";
                await _emailService.SendEmailAsync(Email, emailSubject, htmlBody);
            }
        }
        //send email contains resetpasswordlink
        public async Task<ApiResponse> RequestPasswordResetAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            /*This method requires frontend integration to function correctly.
            The frontend should handle user input, send requests to this API, and display appropriate success or error messages.*/
            var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?email={email}&token={Uri.EscapeDataString(token)}";
            var emailBody = $@"
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title>Password Reset</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 500px;
            margin: 30px auto;
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            text-align: center;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            color: #333;
            margin-bottom: 20px;
        }}
        .message {{
            font-size: 16px;
            color: #555;
            margin-bottom: 20px;
        }}
        .reset-button {{
            display: inline-block;
            background-color: #007bff;
            color: white;
            padding: 12px 20px;
            font-size: 18px;
            font-weight: bold;
            border-radius: 6px;
            text-decoration: none;
            transition: background 0.3s ease-in-out;
        }}
        .reset-button:hover {{
            background-color: #0056b3;
        }}
        .footer {{
            font-size: 14px;
            color: #888;
            margin-top: 20px;
        }}
        .footer a {{
            color: #007bff;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>Reset Your Password</div>
        <p class='message'>Click the button below to reset your password. This link is valid for a limited time.</p>
        <a href='{resetLink}' class='reset-button'>Reset Password</a>
        <p class='message'>If you did not request this, please ignore this email.</p>
        <div class='footer'>Need help? <a href='mailto:michealghobriall@gmail.com'>Contact Support</a></div>
    </div>
</body>
</html>";

            await _emailService.SendEmailAsync(user.Email, "Password Reset", emailBody);
            return new ApiResponse(StatusCodes.Status200OK, "Password reset link has been sent to your email.");

        }
    }
}
