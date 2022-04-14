using System;

namespace ViewModel.Catalog.Company
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int SubCommentId { get; set; }
        public string AvatarPath { get; set; }
        public string Name { get; set; }
    }
}
