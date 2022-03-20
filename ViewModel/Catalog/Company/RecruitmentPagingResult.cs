using System;
using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class RecruitmentPagingResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateCreated { get; set; }
        public int Salary { get; set; }
        public string Rank { get; set; }
        public List<string> Branches { get; set; }
        public string AvatarPath { get; set; }
    }
}
