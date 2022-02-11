using System;
using System.ComponentModel.DataAnnotations;

namespace MVCApp.ViewModels
{
    public class ChangePasswordRequest
    {
        public Guid Id { get; set; }
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
