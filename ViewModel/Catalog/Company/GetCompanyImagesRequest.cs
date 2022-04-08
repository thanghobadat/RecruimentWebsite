using System;
using ViewModel.Common;

namespace ViewModel.Catalog.Company
{
    public class GetCompanyImagesRequest : PagingRequestBase
    {
        public Guid CompanyId { get; set; }
    }
}
