using Microsoft.AspNetCore.Http;

namespace TeaShop.Business
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}