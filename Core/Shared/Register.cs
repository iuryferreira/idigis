using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Shared.Notifications;

namespace Shared
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddLayer (this IServiceCollection services)
        {
            services.AddHelpers();
        }

        private static void AddHelpers (this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
        }
    }
}
