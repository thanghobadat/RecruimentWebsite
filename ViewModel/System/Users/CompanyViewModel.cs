using System;

namespace ViewModel.System.Users
{
    public class CompanyViewModel
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkerNumber { get; set; }
        public string ContactName { get; set; }
        public DateTime DateCreatedAvatar { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public long FizeSize { get; set; }
    }
}
