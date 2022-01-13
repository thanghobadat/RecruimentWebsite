using System.Collections.Generic;

namespace ViewModel.Common
{
    public class PageResult<T> : PagedResultBase
    {
        public List<T> Items { get; set; }

    }
}
