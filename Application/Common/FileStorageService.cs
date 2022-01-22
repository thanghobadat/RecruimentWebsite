using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Application.Common
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolderAvatar;
        private readonly string _userContentFolderImages;
        private const string USER_CONTENT_FOLDER_NAME_Avatar = "Avatar";
        private const string USER_CONTENT_FOLDER_NAME_Images = "Images";

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolderAvatar = Path.GetFullPath($"wwwroot/{USER_CONTENT_FOLDER_NAME_Avatar}");
            _userContentFolderImages = Path.GetFullPath($"wwwroot/{USER_CONTENT_FOLDER_NAME_Images}");
        }

        public string GetFileUrlAvatar(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME_Avatar}/{fileName}";
        }
        public string GetFileUrlImages(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME_Images}/{fileName}";
        }
        public async Task SaveAvatarAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolderAvatar, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task SaveImagesAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolderImages, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task DeleteAvatarAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolderAvatar, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
        public async Task DeleteImagesAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolderImages, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

    }
}
