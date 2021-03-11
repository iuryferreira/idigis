using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Authentication.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Core.Authentication.Services
{
    public class JwtService : IJwtService
    {
        public string GenerateToken (string email, string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
                    new(new[]
                    {
                        new(ClaimTypes.Email, email), new Claim(ClaimTypes.Name, id),
                        new(ClaimTypes.Uri, Settings.ServerUrl)
                    }),
                Expires =
                    DateTime.UtcNow.AddHours(Settings.JwtExpirationInHours),
                SigningCredentials = new(new SymmetricSecurityKey(Settings.Key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
