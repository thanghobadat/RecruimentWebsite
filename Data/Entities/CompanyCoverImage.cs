using System;

namespace Data.Entities
{
    public class CompanyCoverImage
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public long FizeSize { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
    }
}
