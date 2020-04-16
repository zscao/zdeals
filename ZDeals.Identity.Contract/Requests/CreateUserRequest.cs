namespace ZDeals.Identity.Contract.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Nickname { get; set; }

        public string Role { get; set; }
    }
}
