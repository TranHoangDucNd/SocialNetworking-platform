using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using WebDating.Entities;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
                (
                    config.Value.CloudName,
                    config.Value.ApiKey,
                    config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);

        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();//mở luồng y/c để đọc tệp đã tải lên
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "dating-web"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<IEnumerable<ImageUploadResult>> AddRangerPhotoAsync(List<IFormFile> files)
        {
            var uploadResults = new List<ImageUploadResult>();
            foreach(var image in files)
            {
                uploadResults.Add(await AddPhotoAsync(image));
            }
            return uploadResults;
        }

        public async Task DeleteRangerPhotoAsync(ICollection<ImagePost> images)
        {
            foreach(var image in images)
            {
                await DeletePhotoAsync(image.PublicId);
            }
        }
    }
}
