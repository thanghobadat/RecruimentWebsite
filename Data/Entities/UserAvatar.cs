using System;

namespace Data.Entities
{
    public class UserAvatar
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public long FizeSize { get; set; }
        public Guid UserId { get; set; }
        public UserInformation UserInformation { get; set; }

    }
}
