using System;
using System.Text;

namespace Core.Authentication
{
    public static class Settings
    {
        private static readonly string Secret = Environment.GetEnvironmentVariable("JwtSecret");
        public static readonly byte[] Key = Encoding.ASCII.GetBytes(Secret);
        public static readonly string ServerUrl = Environment.GetEnvironmentVariable("ServerUrl");

        public static readonly double JwtExpirationInHours =
            Double.Parse(Environment.GetEnvironmentVariable("JwtExpirationInHours") ?? string.Empty);
    }
}
