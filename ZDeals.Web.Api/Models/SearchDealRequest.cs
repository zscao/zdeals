namespace ZDeals.Web.Api.Models
{
    public class SearchDealRequest
    {
        public string Category { get; set; }
        public string Keywords { get; set; }
        public int pageNumber { get; set; }
    }
}
