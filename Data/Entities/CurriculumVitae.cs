using System;

namespace Data.Entities
{
    public class CurriculumVitae
    {
        public int RecruimentId { get; set; }
        public Guid UserId { get; set; }
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public Recruitment Recruitment { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
