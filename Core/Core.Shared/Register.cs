using System.Diagnostics.CodeAnalysis;
using Core.Shared.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddHelpers (this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
        }
    }
}
