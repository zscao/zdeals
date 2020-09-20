namespace ZDeals.Api.Contract.Requests
{
    public class UpdateCategoryRequest
    {
        public string Title { get; set; }

        public int ParentId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
