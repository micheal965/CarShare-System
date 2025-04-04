using Microsoft.AspNetCore.Mvc;
using Youth_Innovation_System.Core.IServices.Email;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Email;

namespace Youth_Innovation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("Send-Email")]
        public async Task<IActionResult> SendEmail(SendEmailDto sendEmailDto)
        {
            await _emailService.SendEmailAsync(sendEmailDto.to, sendEmailDto.subject, sendEmailDto.body);
            return Ok(new ApiResponse(StatusCodes.Status200OK, "Email Sent Successfully!"));
        }



    }
}
