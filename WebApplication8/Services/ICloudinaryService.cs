using CloudinaryDotNet.Actions;

namespace WebApplication8.Services
{
    public interface ICloudinaryService
    {
        public Task<ImageUploadResult> UploadImage(IFormFile file);
        public Task<string> GetImageUrl(string publicId, int width = 0, int height = 0);
    }
}
