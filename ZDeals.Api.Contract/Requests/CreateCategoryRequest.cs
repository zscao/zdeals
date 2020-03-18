namespace ZDeals.Api.Contract.Requests
{
    public class CreateCategoryRequest
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int DisplayOrder { get; set; }
    }
}
