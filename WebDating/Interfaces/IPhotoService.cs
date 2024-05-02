using CloudinaryDotNet.Actions;
using WebDating.Entities.PostEntities;

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
