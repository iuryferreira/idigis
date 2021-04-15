using System.Diagnostics.CodeAnalysis;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Application.UseCases;
using Idigis.Core.Persistence;
using Idigis.Core.Persistence.Contracts;
using Idigis.Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notie;
using Notie.Contracts;

namespace Idigis.Core
{
    [ExcludeFromCodeCoverage]
    public static class Register
    {
        public static void AddCore (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddNotification();
            services.AddApplication();
            services.AddPersistence(configuration);
        }

        private static void AddNotification (this IServiceCollection services)
        {
            services.AddScoped<AbstractNotificator, Notificator>();
        }

        private static void AddPersistence (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IChurchRepository, ChurchRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ITitheRepository, TitheRepository>();
        }

        private static void AddApplication (this IServiceCollection services)
        {
            services.AddScoped<IChurchUseCase, ChurchUseCase>();
            services.AddScoped<IMemberUseCase, MemberUseCase>();
            services.AddScoped<IOfferUseCase, OfferUseCase>();
            services.AddScoped<ITitheUseCase, TitheUseCase>();
        }
    }
}
