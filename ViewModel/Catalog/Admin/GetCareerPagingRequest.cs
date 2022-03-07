using ViewModel.Common;

namespace ViewModel.Catalog.Admin
{
    public class GetCareerPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

    }
}
