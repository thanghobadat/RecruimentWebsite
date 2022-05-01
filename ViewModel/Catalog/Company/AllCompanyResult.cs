using System;
using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class AllCompanyResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CountRecruitment { get; set; }
        public List<CompanyBranchViewModel> Branches { get; set; }
        public string AvatarPath { get; set; }
        public int WorkerNumber { get; set; }
    }
}
