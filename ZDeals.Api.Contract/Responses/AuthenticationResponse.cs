using ZDeals.Api.Contract.Models;

namespace ZDeals.Api.Contract.Responses
{
    public class AuthenticationResponse
    {
        public User User { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
