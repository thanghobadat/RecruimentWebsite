using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;

namespace Application.Common
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolderAvatar;
        private readonly string _userContentFolderImages;
        private readonly string _userContentFolderCoverImage;
        private readonly string _userContentFolderCV;
        private const string USER_CONTENT_FOLDER_NAME_Avatar = "Avatars";
        private const string USER_CONTENT_FOLDER_NAME_Images = "Images";
        private const string USER_CONTENT_FOLDER_NAME_Cover_Images = "CoverImages";
        private const string USER_CONTENT_FOLDER_NAME_CV = "CVs";

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolderAvatar = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME_Avatar);
            _userContentFolderImages = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME_Images);
            _userContentFolderCoverImage = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME_Cover_Images);
            _userContentFolderCV = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME_CV);
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

        public async Task SaveCoverImageAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolderCoverImage, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteCoverImageAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolderCoverImage, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task SaveCVAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolderCV, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteCVAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolderCV, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public DownloadFileViewModel DownloadZip(string fileName)
        {
            var filePath = Path.Combine(_userContentFolderCV, fileName);
            var nameFile = string.Concat(Path.GetFileName(filePath), ".zip");
            using (var memoryStream = new MemoryStream())
            {
                using (var achive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
                {
                    achive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }
                var fileDownload = new DownloadFileViewModel()
                {
                    FiltType = "aplication/zip",
                    ArchiveData = memoryStream.ToArray(),
                    AchiveName = nameFile

                };
                return fileDownload;
            }
        }
    }
}


