using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Hash;
using Idigis.Api.Auth.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.IdentityModel.Tokens;
using Notie.Contracts;
using Notie.Models;

namespace Idigis.Api.Auth
{
    public class AuthService : IAuthService
    {
        public AuthService (AbstractNotificator notificator)
        {
            Notificator = notificator;
        }

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
                            new(ClaimTypes.Uri, AuthSettings.ServerUrl)
                        }),
                    Expires =
                        DateTime.UtcNow.AddHours(double.Parse(Environment.GetEnvironmentVariable("JwtExpirationInHours") ?? string.Empty)),
                    SigningCredentials =
                        new(new SymmetricSecurityKey(AuthSettings.Key), SecurityAlgorithms.HmacSha256Signature)
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
