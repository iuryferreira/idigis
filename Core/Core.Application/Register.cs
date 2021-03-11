using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Application.Contracts;
using Core.Application.Handlers;
using Core.Authentication;
using Core.Persistence;
using Core.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddApplication (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddHandlers();
            services.AddHelpers();
            services.AddAuthLayer();
        }

        private static void AddHandlers (this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<ICreateChurchHandler, CreateChurchHandler>();
            services.AddScoped<ILoginHandler, LoginHandler>();
        }
    }
}
