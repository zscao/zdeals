using System;
using System.Collections.Generic;
using System.Text;

namespace ZDeals.Common.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }
        public TimeSpan TokenLifetime { get; set; }
        public TimeSpan RefreshTokenLifetime { get; set; }
    }
}
