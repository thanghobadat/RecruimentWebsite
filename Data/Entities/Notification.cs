using System;

namespace Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public AppUser AppUser { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
