using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Service
{
    [ExcludeFromCodeCoverage]
    public static class Idigis
    {
        public static void Add (this IServiceCollection services, string connectionString)
        {
            Shared.Register.AddLayer(services);
            Persistence.Register.AddLayer(services, connectionString);
            Application.Register.AddLayer(services);
        }
    }
}
