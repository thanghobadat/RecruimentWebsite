using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Recruitment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Experience { get; set; }
        public string DetailedExperience { get; set; }
        public string Benefits { get; set; }
        public int Salary { get; set; }
        public string Education { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CareerRecruitment> CareerRecruitments { get; set; }
        public List<BranchRecruitment> BranchRecruiments { get; set; }
        public List<CurriculumVitae> CurriculumVitaes { get; set; }
        public List<Comment> Comments { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyInformation CompanyInformation { get; set; }




    }
}
