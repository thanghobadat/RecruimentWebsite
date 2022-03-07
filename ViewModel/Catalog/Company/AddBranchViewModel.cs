using System;

namespace ViewModel.Catalog.Company
{
    public class AddBranchViewModel
    {
        public Guid CompanyId { get; set; }
        public int BranchId { get; set; }
        public string Address { get; set; }
    }
}
