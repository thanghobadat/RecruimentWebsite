using System.Threading.Tasks;
using ViewModel.System.Users;

namespace MVCApp.Service
{
    public interface IAccountApiClient
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
