using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime DateCreated { get; set; }
        public bool IsSave { get; set; }
        public UserInformation UserInformation { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
