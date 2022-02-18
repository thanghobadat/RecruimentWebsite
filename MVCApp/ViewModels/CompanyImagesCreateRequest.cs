using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCApp.ViewModels
{
    public class CompanyImagesCreateRequest
    {
        public Guid CompanyId { get; set; }
        [Display(Name = "Choose Images")]
        public List<IFormFile> Images { get; set; }
    }
}
