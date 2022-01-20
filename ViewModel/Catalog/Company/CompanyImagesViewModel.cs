using System;

namespace ViewModel.Catalog.Company
{
    public class CompanyImagesViewModel
    {
        public Guid CompanyId { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public long FizeSize { get; set; }
    }
}
