using System;

namespace Data.Entities
{
    public class CompanyBranch
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
    }
}
