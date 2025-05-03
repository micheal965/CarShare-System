using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.IRepositories;
using Youth_Innovation_System.Core.IServices.Cloudinary;
using Youth_Innovation_System.Core.IServices.Rental;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Core.Specifications.RentalSpecifications;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Rental;
using Youth_Innovation_System.Shared.Exceptions;

namespace Youth_Innovation_System.Service.RentalServices
{
    public class RentalApplicationService : IRentalApplicationService
    {
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalApplicationService(ICloudinaryServices cloudinaryServices,
                                            IUnitOfWork unitOfWork,
                                            IMapper mapper)

        {
            _cloudinaryServices = cloudinaryServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> ApplyForRentalAsync(string renterId, int carPostId, IFormFile licenseFile, IFormFile proposalFile)
        {
            // Upload the files to Cloudinary
            List<RawUploadResult> uploadResults;
            try
            {
                uploadResults = await _cloudinaryServices.UploadFilesAsync(new List<IFormFile>() { licenseFile, proposalFile });
            }
            catch (ArgumentException ex)
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }

            var licenseFileUrl = uploadResults.FirstOrDefault()?.Url.ToString();
            var proposalFileUrl = uploadResults.Skip(1).FirstOrDefault()?.Url.ToString();

            if (string.IsNullOrEmpty(licenseFileUrl) || string.IsNullOrEmpty(proposalFileUrl))
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, $"Failed to upload required files to Cloudinary.");
            }

            // Create a new RentalApplication record
            var rentalApplication = new RentalApplication
            {
                RenterId = renterId,
                PostId = carPostId,
                LicenseFileUrl = licenseFileUrl,
                ProposalFileUrl = proposalFileUrl,
                ApplicationDate = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.Repository<RentalApplication>().AddAsync(rentalApplication);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, $"Error saving rental application: {ex.Message}");
            }
            return new ApiResponse(StatusCodes.Status200OK, "Rental application submitted successfully.");
        }

        public async Task<IReadOnlyList<RentalApplicationDto>> ReviewAllPendingApplicationsAsync(string userId, int CarPostId)
        {
            GetPostForShowingPendingAppsSpecifications spec = new GetPostForShowingPendingAppsSpecifications(userId, CarPostId);

            var post = await _unitOfWork.Repository<CarPost>().GetWithSpecAsync(spec);
            if (post == null) throw new NotFoundException("No post found");

            var RentalApps = post.RentalApplications.Where(RA => RA.Status == RentalAppStatus.Pending.ToString()).ToList();

            return _mapper.Map<IReadOnlyList<RentalApplicationDto>>(RentalApps);
        }

        public async Task<ApiResponse> ReviewRentalApplicationAsync(string userId, int applicationId, bool isAccepted)
        {
            var RentalApplicationRepo = _unitOfWork.Repository<RentalApplication>();

            var application = await RentalApplicationRepo.GetAsync(applicationId);
            if (application == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "Rental application not found.");

            var CarPostRepo = _unitOfWork.Repository<CarPost>();

            var car = await CarPostRepo.GetAsync(application.PostId);
            if (car == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "Car post not found.");

            // Check if the user is the owner of the car
            if (car.OwnerId != userId)
                return new ApiResponse(StatusCodes.Status401Unauthorized, "You are not authorized to review this rental application.");

            // Update the status based on acceptance
            application.Status = isAccepted ? RentalAppStatus.Approved.ToString() : RentalAppStatus.Rejected.ToString();

            // Mark the car as rented if accepted
            if (isAccepted) car.RentalStatus = CarStatus.Rented.ToString();

            try
            {
                RentalApplicationRepo.Update(application);
                CarPostRepo.Update(car);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse(StatusCodes.Status200OK, isAccepted ? "Application accepted and car rented." : "Application rejected.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        public async Task<ApiResponse> SubmitCarFeedbackAsync(string userId, int carPostId, int rating, string feedback)
        {
            SubmitFeedbackSpecifications spec = new SubmitFeedbackSpecifications(userId, carPostId);
            var rentalHistory = await _unitOfWork.Repository<RentalApplication>().GetWithSpecAsync(spec);

            if (rentalHistory == null)
                return new ApiResponse(StatusCodes.Status403Forbidden, "You can only provide feedback for cars you have rented.");
            var carFeedback = new CarFeedback()
            {
                CarPostId = carPostId,
                RenterId = userId,
                Rating = rating,
                FeedbackText = feedback
            };
            try
            {
                await _unitOfWork.Repository<CarFeedback>().AddAsync(carFeedback);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse(StatusCodes.Status200OK, "Feedback submitted successfully.");

            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, ex.Message);
            }

        }
    }
}
