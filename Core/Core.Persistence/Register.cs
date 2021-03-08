using System.Diagnostics.CodeAnalysis;
using Core.Persistence.Contexts;
using Core.Persistence.Contracts;
using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Persistence
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
