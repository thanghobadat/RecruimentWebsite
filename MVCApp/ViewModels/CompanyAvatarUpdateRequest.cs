﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MVCApp.ViewModels
{
    public class CompanyAvatarUpdateRequest
    {
        public int Id { get; set; }
        [Display(Name = "Choose Image")]
        public IFormFile ThumnailImage { get; set; }
    }
}