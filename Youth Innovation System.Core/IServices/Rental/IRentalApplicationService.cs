using Microsoft.AspNetCore.Http;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Rental;

namespace Youth_Innovation_System.Core.IServices.Rental
{
    public interface IRentalApplicationService
    {
        Task<ApiResponse> ApplyForRentalAsync(string renterId, int carPostId, IFormFile licenseFile, IFormFile proposalFile);
        Task<ApiResponse> ReviewRentalApplicationAsync(string userId, int applicationId, bool isAccepted);
        Task<IReadOnlyList<RentalApplicationResponseDto>> ReviewAllPendingApplicationsAsync(string userId, int CarPostId);
        Task<ApiResponse> SubmitCarFeedbackAsync(string userId, int carPostId, int rating, string feedback);
    }
}
