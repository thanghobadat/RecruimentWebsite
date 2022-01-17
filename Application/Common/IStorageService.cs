using System.IO;
using System.Threading.Tasks;

namespace Application.Common
{
    //save file và lấy thông tin file
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}
