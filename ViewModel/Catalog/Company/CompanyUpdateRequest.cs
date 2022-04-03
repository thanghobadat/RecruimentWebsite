using System;

namespace ViewModel.Catalog.Company
{
    public class CompanyUpdateRequest
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkerNumber { get; set; }
        public string ContactName { get; set; }
    }
}
