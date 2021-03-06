using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Application.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddLayer (this IServiceCollection services)
        {
            services.AddHandlers();
        }

        private static void AddHandlers (this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<ICreateChurchHandler, CreateChurchHandler>();
        }
    }
}
