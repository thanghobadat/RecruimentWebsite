using Microsoft.AspNetCore.Http;
using System;

namespace ViewModel.Catalog.Company
{
    public class CreateCoverImageRequest
    {
        public Guid CompanyId { get; set; }
        public IFormFile ThumnailIage { get; set; }
    }
}
