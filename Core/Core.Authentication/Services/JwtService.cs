using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Authentication.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Core.Authentication.Services
{
    public class JwtService : IJwtService
    {
        public string GenerateToken (string email, string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JwtSecret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
                    new(new[]
                    {
                        new(ClaimTypes.Email, email), new Claim(ClaimTypes.Name, id),
                        new(ClaimTypes.Uri, Environment.GetEnvironmentVariable("ServerUrl"))
                    }),
                Expires =
                    DateTime.UtcNow.AddHours(
                        Double.Parse(Environment.GetEnvironmentVariable("JwtExpirationInHours"))),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
