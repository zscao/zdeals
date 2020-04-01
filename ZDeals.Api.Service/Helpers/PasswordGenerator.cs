using System;
using System.Linq;

namespace ZDeals.Api.Service.Helpers
{
    public sealed class PasswordGenerator
    {
        const string pwdChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        public string GenerateRandomPassword(int passwordLength)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(pwdChars, passwordLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
