using Microsoft.AspNetCore.Http;
using System;

namespace ViewModel.Catalog.Company
{
    public class CreateCompanyImageRequest
    {
        public Guid CompanyId { get; set; }
        public IFormFile Image { get; set; }
    }
}
