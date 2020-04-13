namespace ZDeals.Api.Contract.Requests
{
    public class SearchDealRequest
    {
        public string Category { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public bool? Deleted { get; set; }
    }
}
