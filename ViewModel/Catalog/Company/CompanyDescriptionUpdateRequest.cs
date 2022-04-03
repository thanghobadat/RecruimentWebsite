using System;

namespace ViewModel.Catalog.Company
{
    public class CompanyDescriptionUpdateRequest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
