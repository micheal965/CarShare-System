using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Youth_Innovation_System.Core.IServices.Rental;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Rental;

namespace Youth_Innovation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalApplicationService _rentalApplicationService;

        public RentalController(IRentalApplicationService rentalApplicationService)
        {
            _rentalApplicationService = rentalApplicationService;
        }
        [Authorize(Roles = nameof(UserRoles.Renter))]
        [HttpPost("Apply")]
        public async Task<IActionResult> ApplyForRental(RentalApplicationDto rentalApplicationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _rentalApplicationService.ApplyForRentalAsync(userId,
                                                       rentalApplicationDto.CarId,
                                                       rentalApplicationDto.licenseFile,
                                                       rentalApplicationDto.proposalFile);
            return StatusCode(result.StatusCode, result);

        }
        [Authorize(Roles = nameof(UserRoles.CarOwner))]
        [HttpPut("Review")]
        public async Task<IActionResult> ReviewRentalApplication(ReviewApplicationDto reviewApplicationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _rentalApplicationService.ReviewRentalApplicationAsync(userId,
                                                                             reviewApplicationDto.applicationId,
                                                                             reviewApplicationDto.isAccepted);
            return StatusCode(result.StatusCode, result);

        }

        [Authorize(Roles = nameof(UserRoles.Renter))]
        [HttpPost("Submit-Feedback")]
        public async Task<IActionResult> SubmitFeedback(SubmitFeedbackDto submitFeedbackDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _rentalApplicationService.SubmitCarFeedbackAsync(userId,
                                                                             submitFeedbackDto.carPostId,
                                                                             submitFeedbackDto.rating,
                                                                             submitFeedbackDto.feedback);
            return StatusCode(result.StatusCode, result);

        }

        [Authorize(Roles = nameof(UserRoles.CarOwner))]
        [HttpGet("Get-All-Pending-RentalApps/{carPostId}")]
        public async Task<IActionResult> GetAllPendingRentalApps(int carPostId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Ok(await _rentalApplicationService.ReviewAllPendingApplicationsAsync(userId, carPostId));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
    }
}
