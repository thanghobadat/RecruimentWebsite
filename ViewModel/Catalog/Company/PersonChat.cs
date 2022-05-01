using System;

namespace ViewModel.Catalog.Company
{
    public class PersonChat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AvatarPath { get; set; }
        public string LastContent { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
