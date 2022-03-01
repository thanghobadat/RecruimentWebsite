using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class CareerRecruitment
    {
        public int RecruimentId { get; set; }
        public int CareerId { get; set; }
        public Recruitment Recruitment { get; set; }
        public Career Career { get; set; }
    }
}
