using System;

namespace ViewModel.Catalog.Company
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Performer { get; set; }
    }
}
