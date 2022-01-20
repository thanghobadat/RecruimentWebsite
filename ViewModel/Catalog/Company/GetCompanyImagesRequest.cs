using System;
using ViewModel.Common;

namespace ViewModel.Catalog.Company
{
    public class GetCompanyImagesRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public Guid CompanyId { get; set; }
    }
}
