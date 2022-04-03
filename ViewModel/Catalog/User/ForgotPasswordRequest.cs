using System;

namespace ViewModel.Catalog.User
{
    public class ForgotPasswordRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
