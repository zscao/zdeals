using System.Collections.Generic;

namespace ZDeals.Api.Contract
{
    public class PagedData<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
