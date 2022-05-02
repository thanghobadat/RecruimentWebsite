using System;

namespace ViewModel.System.Users
{
    public class ChangePasswordRequest
    {
        public Guid Id { get; set; }
        public string NewPassword { get; set; }
    }
}
