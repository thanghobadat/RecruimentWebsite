using Microsoft.AspNetCore.Http;

namespace ViewModel.Catalog.Company
{
    public class UpdateCoverImageRequest
    {
        public IFormFile ThumnailImage { get; set; }
    }
}
