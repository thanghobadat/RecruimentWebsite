using System;

namespace ViewModel.Catalog.Company
{
    public class CompanyImagesViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public long FileSize { get; set; }
    }
}
