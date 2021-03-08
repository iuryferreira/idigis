using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Application.Handlers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Shared;

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
        }

        private static void AddHandlers (this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<ICreateChurchHandler, CreateChurchHandler>();
        }
    }
}
