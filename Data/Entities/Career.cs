using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Career
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CareerRecruitment> CareerRecruitments { get; set; }

    }
}
