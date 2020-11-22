namespace ZDeals.Api.Contract.Requests
{
    public class AddPricesRequest
    {
        public string DealSource { get; set; }
        public Models.DealPrice[] Items { get; set; }
    }
}
