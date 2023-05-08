using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace WebApplication8.Services
{
    public class CloudinaryService: ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var account = new Account(
                "dbwudkncb", "951251214358913", "RvPLrQwx7Hcn3BfFdGquzzgyEaI"
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = Guid.NewGuid().ToString(),
                Folder = "my_folder"
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<string> GetImageUrl(string publicId, int width = 0, int height = 0)
        {
            var transformation =  new Transformation().Width(width).Height(height).Crop("fill");
            return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
        }
    }
}
