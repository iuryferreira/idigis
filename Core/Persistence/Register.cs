using System.Diagnostics.CodeAnalysis;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Contracts;
using Persistence.Repositories;

namespace Persistence
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddLayer (this IServiceCollection services, string connectionString)
        {
            services.AddPersistence(connectionString);
        }

        private static void AddPersistence (this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IChurchRepository, ChurchRepository>();
            services.AddDbContext<IChurchContext, ChurchContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

        }
    }
}
