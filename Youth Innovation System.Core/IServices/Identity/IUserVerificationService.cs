using Youth_Innovation_System.Shared.ApiResponses;

namespace Youth_Innovation_System.Core.IServices.IdentityServices
{
    public interface IUserVerificationService
    {
        Task RequestVerificationEmailAsync(string Email);
        Task<ApiResponse> RequestPasswordResetAsync(string email);
    }
}
