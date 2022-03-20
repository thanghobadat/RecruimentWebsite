using System;

namespace ViewModel.Catalog.Company
{
    public class CommentRequest
    {
        public Guid AccountId { get; set; }
        public int RecruitmentId { get; set; }
        public string Content { get; set; }
        public int SubCommentId { get; set; }
        public string Role { get; set; }
    }
}
