using ZDeals.Identity.Contract.Models;

namespace ZDeals.Identity.Contract.Responses
{
    public class JwtResponse
    {
        public User User { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
