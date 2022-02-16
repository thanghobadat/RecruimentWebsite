using Microsoft.AspNetCore.Http;

namespace ViewModel.Catalog.Company
{
    public class AvatarUpdateRequest
    {
        public IFormFile ThumnailImage { get; set; }
    }
}
