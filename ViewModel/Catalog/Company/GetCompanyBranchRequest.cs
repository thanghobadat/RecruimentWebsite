using ViewModel.Common;

namespace ViewModel.Catalog.Company
{
    public class GetCompanyBranchRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
