using System;

namespace Data.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public UserInformation UserInformation { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Performer { get; set; }
    }
}
