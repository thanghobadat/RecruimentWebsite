using System;

namespace MVCApp.ViewModels
{
    public class CompanyBranchCreateRequest
    {
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
    }
}
