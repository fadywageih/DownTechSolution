using Microsoft.AspNetCore.Http;

namespace ServicesAbstraction
{
    public interface IExtendedImageService : IImageService
    {
        Task<string> SaveImageAsync(IFormFile image, string folderName);
        Task<bool> DeleteImageAsync(string imageUrl);

        Task<string> SaveVideoAsync(IFormFile video, string folderName);
        Task<bool> DeleteVideoAsync(string videoUrl);

        Task<string> SaveFileAsync(IFormFile file, string folderName, string[] allowedExtensions);

        string[] GetAllowedImageExtensions();
        string[] GetAllowedVideoExtensions();
    }
}
