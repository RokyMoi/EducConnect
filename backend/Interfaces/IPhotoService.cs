using CloudinaryDotNet.Actions;

namespace EduConnect.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<ImageUploadResult> AddPhotoForPost(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicID);
    }
}
