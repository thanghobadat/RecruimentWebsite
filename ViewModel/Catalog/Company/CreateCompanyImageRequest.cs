using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class CreateCompanyImageRequest
    {
        public Guid CompanyId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
