using System;

namespace ViewModel.Catalog.Company
{
    public class CompanyCoverImageViewModel
    {
        public int Id { get; set; }
        public long FileSize { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateCreated { get; set; }
        public string Caption { get; set; }
        public Guid CompanyId { get; set; }
    }
}
