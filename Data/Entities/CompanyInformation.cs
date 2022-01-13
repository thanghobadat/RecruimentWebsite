using System;

namespace Data.Entities
{
    public class CompanyInformation
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkerNumber { get; set; }
        public string ContactName { get; set; }

        public AppUser AppUser { get; set; }
    }
}
