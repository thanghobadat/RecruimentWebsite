using System.IO;
using System.Threading.Tasks;

namespace Application.Common
{
    //save file và lấy thông tin file
    public interface IStorageService
    {
        string GetFileUrlAvatar(string fileName);
        string GetFileUrlImages(string fileName);

        Task SaveAvatarAsync(Stream mediaBinaryStream, string fileName);
        Task SaveImagesAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteAvatarAsync(string fileName);
        Task DeleteImagesAsync(string fileName);
    }
}
