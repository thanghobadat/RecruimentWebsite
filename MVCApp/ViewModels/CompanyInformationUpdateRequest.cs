using System;

namespace MVCApp.ViewModels
{
    public class CompanyInformationUpdateRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkerNumber { get; set; }
        public string ContactName { get; set; }
    }
}
