using ViewModel.Common;

namespace ViewModel.Catalog.Company
{
    public class GetRecruitmentRequest : PagingRequestBase
    {
        public string Rank { get; set; }
        public string Experience { get; set; }
        public int Salary { get; set; }
        public string Education { get; set; }
        public string Type { get; set; }
        public int BranchId { get; set; }
        public int CareerId { get; set; }


    }
}
