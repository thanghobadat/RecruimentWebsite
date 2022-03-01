using System;

namespace Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public UserInformation UserInformation { get; set; }
        public int RecruimentId { get; set; }
        public Recruitment Recruitment { get; set; }
        public string Content { get; set; }
        public string SubcommentId { get; set; }
    }
}
