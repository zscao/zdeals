namespace ZDeals.Api.Contract.Requests
{
    public class SearchDealRequest
    {
        public string Category { get; set; }
        
        public string Keywords { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public bool? Verified { get; set; }
        
        public bool? Deleted { get; set; }

        public string Store { get; set; }
    }
}
