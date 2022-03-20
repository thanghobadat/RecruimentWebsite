using Microsoft.AspNetCore.Http;
using System;

namespace ViewModel.Catalog.User
{
    public class SubmitCVRequest
    {
        public int RecruitmentId { get; set; }
        public Guid UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
