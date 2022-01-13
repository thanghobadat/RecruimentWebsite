using Data.Enum;
using System;

namespace ViewModel.System.Users
{
    public class UserViewModel : AccountViewModel
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string AcademicLevel { get; set; }
        public int Age { get; set; }

        public Sex Sex { get; set; }
        public string Role { get; set; }
    }
}
