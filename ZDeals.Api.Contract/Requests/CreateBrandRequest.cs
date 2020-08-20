namespace ZDeals.Api.Contract.Requests
{
    public class CreateBrandRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
