using System.Net.Http;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Core.Persistence.Contracts;
using External.Server;
using Tests.Database.Contexts;

namespace Tests
{
    public abstract class IntegrationTest
    {
        protected IntegrationTest ()
        {
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IChurchContext>();
                    services.AddScoped<IChurchContext, MockedChurchContext>();
                });
            });
            Client = factory.CreateClient();
        }

        protected HttpClient Client { get; }
    }
}
