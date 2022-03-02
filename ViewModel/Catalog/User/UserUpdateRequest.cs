using Data.Enum;
using System;

namespace ViewModel.Catalog.User
{
    public class UserUpdateRequest
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string AcademicLevel { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
