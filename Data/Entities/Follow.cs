using System;

namespace Data.Entities
{
    public class Follow
    {
        public Guid UserId { get; set; }
        public UserInformation UserInformation { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
    }
}
