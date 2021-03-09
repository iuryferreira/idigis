using System.Diagnostics.CodeAnalysis;
using Core.Authentication.Contracts;
using Core.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Core.Authentication
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddAuthLayer (this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Settings.Key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
