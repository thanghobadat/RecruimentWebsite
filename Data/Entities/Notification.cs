using System;

namespace Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public UserInformation UserInformation { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
