using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Contracts;
using Persistence.Repositories;

namespace Persistence
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddPersistence (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IChurchRepository, ChurchRepository>();
            services.AddDbContext<IChurchContext, ChurchContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });
        }
    }
}
