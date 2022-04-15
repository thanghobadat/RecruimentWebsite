using System;
using System.Collections.Generic;

namespace ViewModel.Catalog.Company
{
    public class ListCompanyRecruitment
    {
        public int Id { get; set; }
        public List<string> Careers { get; set; }
        public List<string> Branches { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Experience { get; set; }
        public int Salary { get; set; }
        public string Education { get; set; }
        public string Type { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
