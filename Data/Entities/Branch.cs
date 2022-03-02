using System.Collections.Generic;

namespace Data.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string City { get; set; }
        public List<CompanyBranch> CompanyBranches { get; set; }
        public List<BranchRecruitment> BranchRecruiments { get; set; }
    }
}
