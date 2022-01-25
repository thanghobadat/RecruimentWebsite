using System;

namespace ViewModel.System.Users
{
    public class CompanyAccountViewModel
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
