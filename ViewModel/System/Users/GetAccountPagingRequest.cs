using ViewModel.Common;

namespace ViewModel.System.Users
{
    public class GetAccountPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
