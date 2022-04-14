using System;
using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class CompanyRecruitmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> RecruitmentBranches { get; set; }
        public int Salary { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDate { get; set; }
    }
}
