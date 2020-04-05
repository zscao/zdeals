using System;

namespace ZDeals.Identity.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }
        public TimeSpan TokenLifetime { get; set; }
        public TimeSpan RefreshTokenLifetime { get; set; }
    }

}
