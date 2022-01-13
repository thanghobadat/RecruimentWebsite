using Microsoft.AspNetCore.Identity;
using System;

namespace Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime DateCreated { get; set; }
        public string Address { get; set; }
        public bool IsSave { get; set; }
        public UserInformation UserInformation { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
    }
}
