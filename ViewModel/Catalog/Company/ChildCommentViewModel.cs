using System;

namespace ViewModel.Catalog.Company
{
    public class ChildCommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string AvatarPath { get; set; }
        public string Name { get; set; }
    }
}
