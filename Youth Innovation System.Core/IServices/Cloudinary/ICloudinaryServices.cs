using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
namespace Youth_Innovation_System.Core.IServices.Cloudinary
{
    public interface ICloudinaryServices
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string ImageUrl);
        Task<string> GetImageAsync(string ImageUrl);
        Task<List<(string imageUrl, string publicId)>> UploadImagesAsync(List<IFormFile> files);
        Task<List<RawUploadResult>> UploadFilesAsync(List<IFormFile> files);
        Task<bool> DeleteImagesAsync(List<string> imagesUrls);


    }
}
