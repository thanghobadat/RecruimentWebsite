using Data.Enum;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class UserInformation
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string Address { get; set; }
        public string AcademicLevel { get; set; }
        public AppUser AppUser { get; set; }
        public UserAvatar UserAvatar { get; set; }
        public List<CurriculumVitae> CurriculumVitaes { get; set; }
        public List<Follow> Follows { get; set; }
        public List<Chat> Chats { get; set; }

    }
}
