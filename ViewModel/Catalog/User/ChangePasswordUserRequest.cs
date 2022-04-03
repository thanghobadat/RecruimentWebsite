using System;

namespace ViewModel.Catalog.User
{
    public class ChangePasswordUserRequest
    {
        public Guid UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
