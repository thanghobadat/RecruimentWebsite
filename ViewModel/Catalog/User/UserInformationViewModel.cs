using Data.Enum;
using System;

namespace ViewModel.Catalog.User
{
    public class UserInformationViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string Address { get; set; }
        public string AcademicLevel { get; set; }
        public UserAvatarViewModel UserAvatar { get; set; }
    }
}
