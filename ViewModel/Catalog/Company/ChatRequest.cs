using System;

namespace ViewModel.Catalog.Company
{
    public class ChatRequest
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string Content { get; set; }
        public string Performer { get; set; }
    }
}
