using System;
using System.Text;

namespace Idigis.Api.Auth
{
    public static class AuthSettings
    {
        private static readonly string Secret = Environment.GetEnvironmentVariable("JwtSecret");
        public static readonly byte[] Key = Encoding.ASCII.GetBytes(Secret);
        public static readonly string ServerUrl = Environment.GetEnvironmentVariable("ServerUrl");
    }
}
