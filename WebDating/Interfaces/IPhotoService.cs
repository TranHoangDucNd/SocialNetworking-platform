using CloudinaryDotNet.Actions;
using WebDating.Entities;

namespace WebDating.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<IEnumerable<ImageUploadResult>> AddRangerPhotoAsync(List<IFormFile> files);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task DeleteRangerPhotoAsync(ICollection<ImagePost> images);


    }
}
