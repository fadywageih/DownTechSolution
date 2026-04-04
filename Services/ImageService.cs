namespace Services
{
    public class ImageService : IExtendedImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string ImagesFolder = "uploads/products/images";
        private const string VideosFolder = "uploads/products/videos";
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly string[] _allowedVideoExtensions = { ".mp4", ".webm", ".mov", ".avi", ".mkv" };
        private const long MaxVideoSize = 100 * 1024 * 1024; // 100 MB
        private const long MaxImageSize = 10 * 1024 * 1024; // 10 MB

        public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        #region IExtendedImageService Implementation

        public string[] GetAllowedImageExtensions() => _allowedImageExtensions;
        public string[] GetAllowedVideoExtensions() => _allowedVideoExtensions;

        #endregion

        #region IImageService Implementation (Base64)

        public async Task<string> SaveBase64ImageAsync(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image) || !base64Image.StartsWith("data:image"))
                return null;

            try
            {
                var match = Regex.Match(base64Image, @"^data:image/(?<type>[a-zA-Z]+);base64,(?<data>.+)$");
                if (!match.Success)
                    return null;

                var imageType = match.Groups["type"].Value;
                var base64Data = match.Groups["data"].Value;
                var extension = GetFileExtension(imageType);
                var fileName = GenerateFileName(extension);
                var filePath = GetImageFilePath(fileName);

                await EnsureDirectoryExists(ImagesFolder);
                await SaveFileFromBase64(base64Data, filePath);
                return GetImageUrl(fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetImageUrl(string fileName)
        {
            var baseUrl = GetBaseUrl();
            return $"{baseUrl}/{ImagesFolder}/{fileName}";
        }
        public bool DeleteImage(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return false;

                var fileName = Path.GetFileName(imageUrl);
                var filePath = GetImageFilePath(fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Image File Methods

        public async Task<string> SaveImageAsync(IFormFile image, string folderName)
        {
            return await SaveFileAsync(image, folderName ?? ImagesFolder, _allowedImageExtensions, MaxImageSize);
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            return await DeleteFileAsync(imageUrl, ImagesFolder);
        }

        #endregion

        #region Video File Methods

        public async Task<string> SaveVideoAsync(IFormFile video, string folderName)
        {
            return await SaveFileAsync(video, folderName ?? VideosFolder, _allowedVideoExtensions, MaxVideoSize);
        }

        public async Task<bool> DeleteVideoAsync(string videoUrl)
        {
            return await DeleteFileAsync(videoUrl, VideosFolder);
        }

        #endregion

        #region Generic File Methods

        public async Task<string> SaveFileAsync(IFormFile file, string folderName, string[] allowedExtensions)
        {
            return await SaveFileAsync(file, folderName, allowedExtensions, null);
        }
        private async Task<string> SaveFileAsync(IFormFile file, string folderName, string[] allowedExtensions, long? maxSize)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                if (maxSize.HasValue && file.Length > maxSize.Value)
                    return null;
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                    return null;
                var finalFolder = string.IsNullOrEmpty(folderName) ? ImagesFolder : folderName;
                var fileName = GenerateFileName(extension.TrimStart('.'));
                var uploadsPath = Path.Combine(_environment.WebRootPath, finalFolder);
                var filePath = Path.Combine(uploadsPath, fileName);
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                var baseUrl = GetBaseUrl();
                return $"{baseUrl}/{finalFolder}/{fileName}";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<bool> DeleteFileAsync(string fileUrl, string defaultFolder)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                    return false;
                if (fileUrl.StartsWith("blob:", StringComparison.OrdinalIgnoreCase))
                    return true; 

                string fileName;
                if (fileUrl.StartsWith("http"))
                {
                    var uri = new Uri(fileUrl);
                    fileName = Path.GetFileName(uri.LocalPath);
                }
                else
                {
                    fileName = Path.GetFileName(fileUrl);
                }

                var filePath = Path.Combine(_environment.WebRootPath, defaultFolder, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Helpers
        private string GetBaseUrl()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.Request != null)
            {
                return $"{context.Request.Scheme}://{context.Request.Host}";
            }
            // Fallback لو مفيش Context (حالة نادرة)
            return "http://localhost:5000";
        }

        private string GetFileExtension(string imageType)
        {
            return imageType.ToLower() switch
            {
                "jpeg" or "jpg" => "jpg",
                "png" => "png",
                "gif" => "gif",
                "webp" => "webp",
                _ => "jpg"
            };
        }

        private string GenerateFileName(string extension)
        {
            return $"product_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.{extension}";
        }

        private string GetImageFilePath(string fileName)
        {
            return Path.Combine(_environment.WebRootPath, ImagesFolder, fileName);
        }

        private async Task EnsureDirectoryExists(string folder)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, folder);
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);
        }

        private async Task SaveFileFromBase64(string base64Data, string filePath)
        {
            var imageBytes = Convert.FromBase64String(base64Data);
            await File.WriteAllBytesAsync(filePath, imageBytes);
        }

        #endregion
    }
}