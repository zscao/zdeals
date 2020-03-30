namespace ZDeals.Api.Contract.Models
{
    public class DealPicture
    {
        public string FileName { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }

        public bool IsDefaultPicture { get; set; }

        public string Url { get; set; }
    }
}
