using System;

namespace ViewModel.System.Users
{
    public class LoginViewModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Role { get; set; }
    }
}
