using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace MVCApp.ViewModels
{
    public class CompanyCoverImageCreateRequest
    {
        public Guid CompanyId { get; set; }
        [Display(Name = "Choose Image")]
        public IFormFile ThumnailImage { get; set; }
    }
}
