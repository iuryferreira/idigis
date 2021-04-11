using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Idigis.Api;
using Idigis.Core.Persistence;
using Idigis.Tests.IntegrationTests.Persistence.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.IntegrationTests.Api
{
    [TestClass]
    public class IndexControllerTest
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IndexControllerTest ()
        {
            var context = TestContextFactory.CreateDbContext();
            _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<Context>();
                    services.AddScoped(_ => context);
                });
            });
        }

        [TestMethod]
        public async Task The_Signup_Method_Must_Return_a_Bad_Request_When_Receiving_Invalid_Data ()
        {
            var data = new { Name = "", Email = "invalid_email", Password = "pass" };
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync(Routes.Index.Signup, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Signup_Method_Must_Return_Created_When_Receiving_Valid_Data ()
        {
            var data = new { Name = "valid_name", Email = "valid_email@email.com", Password = "valid_password" };
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync(Routes.Index.Signup, data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
