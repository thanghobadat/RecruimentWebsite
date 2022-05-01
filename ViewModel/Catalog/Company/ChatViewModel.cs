using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class ChatViewModel
    {
        public string Name { get; set; }
        public string AvatarPath { get; set; }
        public List<ListChatContent> Content { get; set; }
    }
}
