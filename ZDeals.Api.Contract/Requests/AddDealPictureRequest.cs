namespace ZDeals.Api.Contract.Requests
{
    public class SaveDealPictureRequest
    {
        public string FileName { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }

        public bool IsDefaultPicture { get; set; }
    }
}
