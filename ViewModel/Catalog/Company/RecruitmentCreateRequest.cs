using System;

namespace ViewModel.Catalog.Company
{
    public class RecruitmentCreateRequest
    {
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
        public Guid CompanyId { get; set; }
    }
}
