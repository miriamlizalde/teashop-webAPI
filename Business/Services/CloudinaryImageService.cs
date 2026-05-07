using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using TeaShop.Business.Utilities;

namespace TeaShop.Business.Services
{
    public class CloudinaryImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageService(IOptions<CloudinarySettings> settings)
        {
            var config  = settings.Value;
            var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var validator = new FileValidationHelper(
                new[] { "image/jpeg", "image/png", "image/gif" },
                new[] { ".jpg", ".jpeg", ".png", ".gif" }
            );
            validator.Validate(file);

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File           = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(400).Height(400).Crop("fill")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl?.ToString() ?? string.Empty;
        }
    }
}