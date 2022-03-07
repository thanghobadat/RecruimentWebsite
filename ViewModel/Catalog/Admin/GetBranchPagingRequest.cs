using ViewModel.Common;

namespace ViewModel.Catalog.Admin
{
    public class GetBranchPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
