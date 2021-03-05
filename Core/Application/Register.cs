using System.Diagnostics.CodeAnalysis;
using Application.Handlers;
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
            services.AddScoped<ICreateChurchHandler, CreateChurchHandler>();
        }
    }
}
