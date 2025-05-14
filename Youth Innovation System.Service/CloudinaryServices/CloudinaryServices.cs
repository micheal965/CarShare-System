using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Youth_Innovation_System.Core.IServices.Cloudinary;

namespace Youth_Innovation_System.Service.CloudinaryServices
{
    public class CloudinaryServices : ICloudinaryServices
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryServices(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;

            var publicId = GetPublicIdfromUrl(imageUrl);
            if (string.IsNullOrEmpty(publicId)) return false;

            var deleteParams = new DeletionParams(publicId);

            var imageDestroyResult = await _cloudinary.DestroyAsync(deleteParams);
            return imageDestroyResult.Result == "ok";

        }
        private string GetPublicIdfromUrl(string url)
        {
            Uri uri = new Uri(url);
            string publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath);
            return publicId;
        }
        public async Task<string> GetImageAsync(string ImageUrl)
        {
            try
            {
                var publicId = GetPublicIdfromUrl(ImageUrl);
                var uri = _cloudinary.Api.UrlImgUp.BuildUrl(publicId);
                return uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("There is no Image");
            }
        }
        public async Task<bool> DeleteImagesAsync(List<string> imagesUrls)
        {
            List<string> publicIds = new List<string>();
            foreach (var imagesUrl in imagesUrls)
            {
                publicIds.Add(GetPublicIdfromUrl(imagesUrl));
            }

            if (publicIds == null || publicIds.Count == 0) return true;

            var deletionParams = new DelResParams
            {
                PublicIds = publicIds,
                Invalidate = true
            };

            var result = await _cloudinary.DeleteResourcesAsync(deletionParams);
            return result.Deleted.Count == imagesUrls.Count; // Check if all images were deleted
        }
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            //Restrict send only images
            if (!IsValidImageFile(file))
                throw new Exception($"Invalid file: {file.FileName} (Only images are allowed)");

            var uploadResult = new ImageUploadResult();

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }

        public async Task<List<RawUploadResult>> UploadFilesAsync(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("No files provided");

            var uploadResults = new List<RawUploadResult>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue; // Skip empty files

                try
                {
                    // Open the file stream
                    await using var stream = file.OpenReadStream();

                    // Set upload parameters
                    var uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "Rentals",
                        Type = "upload"
                    };

                    // Upload the file to Cloudinary
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        uploadResults.Add(uploadResult);
                    }
                    else
                    {
                        throw new Exception($"File upload failed for {file.FileName}: {uploadResult.Error?.Message}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error uploading file {file.FileName}: {ex.Message}");
                }
            }

            return uploadResults;
        }
        /// <summary>
        /// ✅ Validates whether the uploaded file is an image.
        /// </summary>
        private bool IsValidImageFile(IFormFile file)
        {
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var allowedMimeTypes = new HashSet<string> { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

            // Check MIME type
            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                return false;

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return false;

            return true;
        }
        public async Task<List<(string imageUrl, string publicId)>> UploadImagesAsync(List<IFormFile> files)
        {
            var imageList = new List<(string imageUrl, string publicId)>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;

                //Restrict send only images
                if (!IsValidImageFile(file))
                    throw new Exception($"Invalid file: {file.FileName} (Only images are allowed)");
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(800).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageList.Add((uploadResult.SecureUrl.ToString(), uploadResult.PublicId));
            }

            return imageList;
        }

    }
}