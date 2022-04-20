using System.IO;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;

namespace Application.Common
{
    //save file và lấy thông tin file
    public interface IStorageService
    {
        string GetFileUrlAvatar(string fileName);
        string GetFileUrlImages(string fileName);

        Task SaveAvatarAsync(Stream mediaBinaryStream, string fileName);
        Task SaveCVAsync(Stream mediaBinaryStream, string fileName);
        Task SaveImagesAsync(Stream mediaBinaryStream, string fileName);
        Task SaveCoverImageAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteAvatarAsync(string fileName);
        Task DeleteImagesAsync(string fileName);
        Task DeleteCVAsync(string fileName);
        Task DeleteCoverImageAsync(string fileName);
        DownloadFileViewModel DownloadZip(string fileName);
    }
}
