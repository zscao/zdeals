using System;
using System.Security.Cryptography;

namespace ZDeals.Identity.Service.Helpers
{
    public sealed class PasswordGenerator
    {
        public string GenerateRandomPassword(int passwordLength)
        {
            var randomNumber = new byte[passwordLength];

            // the RandomNumberGenerator provides more secure than Guid.NewGuid()
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
