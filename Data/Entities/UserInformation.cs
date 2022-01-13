using Data.Enum;
using System;

namespace Data.Entities
{
    public class UserInformation
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string AcademicLevel { get; set; }
        public AppUser AppUser { get; set; }
    }
}
