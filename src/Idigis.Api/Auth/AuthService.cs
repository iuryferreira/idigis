using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hash;
using Idigis.Api.Auth.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.IdentityModel.Tokens;
using Notie.Contracts;

namespace Idigis.Api.Auth
{
    public class AuthService : IAuthService
    {
        private readonly string ServerUrl;

        public AuthService (AbstractNotificator notificator)
        {
            Notificator = notificator;
            ServerUrl = Environment.GetEnvironmentVariable("ServerUrl");
        }

        internal static byte[] Key => Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JwtSecret"));

        public AbstractNotificator Notificator { get; }

        public LoginResponse Authenticate (LoginRequest request)
        {
            if (new Hashio().Check(request.Hash, request.Password))
            {
                var token = GenerateToken(request.Email, request.ChurchId);
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                return new() { Id = request.ChurchId, Email = request.Email, Name = request.Name, Token = token };
            }

            Notificator.SetNotificationType(new("Unauthorized"));
            return null;
        }

        public LoginResponse RefreshToken (LoginRequest request)
        {
            var token = GenerateToken(request.Email, request.ChurchId);
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            
            return new() { Id = request.ChurchId, Email = request.Email, Name = request.Name, Token = token };
        }

        public string GenerateToken (string email, string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject =
                        new(new[]
                        {
                            new(ClaimTypes.Email, email), new Claim(ClaimTypes.PrimarySid, id),
                            new(ClaimTypes.Uri, ServerUrl)
                        }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials =
                        new(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Auth", "Ocorreu um erro na geração do acesso."));
                return "";
            }
        }
    }
}
