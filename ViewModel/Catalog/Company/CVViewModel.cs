using System;
using ViewModel.Catalog.User;

namespace ViewModel.Catalog.Company
{
    public class CVViewModel
    {
        public UserInformationViewModel User { get; set; }
        public DateTime DateSubit { get; set; }
        public string FilePath { get; set; }
        public int RecruitmentId { get; set; }
        public Guid UserId { get; set; }
        public string RecruitmentName { get; set; }

    }
}
