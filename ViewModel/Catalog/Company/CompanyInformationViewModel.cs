using System;
using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class CompanyInformationViewModel
    {

        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkerNumber { get; set; }
        public string ContactName { get; set; }
        public List<CompanyImagesViewModel> CompanyImages { get; set; }
        public List<CompanyBranchViewModel> CompanyBranches { get; set; }
        public List<CompanyRecruitmentViewModel> CompanyRecruitments { get; set; }
        public CompanyAvatarViewModel CompanyAvatar { get; set; }
        public CompanyCoverImageViewModel CompanyCoverImage { get; set; }
    }
}
