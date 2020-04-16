namespace ZDeals.Identity.Contract.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Role { get; set; }
    }
}
