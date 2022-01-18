using System;

namespace ViewModel.Catalog.Company
{
    public class CreateBranchRequest
    {
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
    }
}
