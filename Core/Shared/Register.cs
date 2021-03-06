using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Shared.Notifications;

namespace Shared
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
