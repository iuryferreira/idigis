using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using External.Server.Contracts;
using Core.Persistence.Contracts;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using External.Server;
using Core.Persistence.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Server.Controllers
{
    [TestClass]
    public class ChurchControllerTest
    {
        private Mock<IChurchContext> _context;
        private WebApplicationFactory<Startup> _factory;

        public ChurchControllerTest ()
        {
            _context = new();
            _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IChurchContext>();
                    services.AddScoped<IChurchContext>((service) => _context.Object);
                });
            });
        }

        [TestMethod]
        public async Task Must_Return_a_Bad_Request_When_Trying_to_Register_a_Church_with_Wrong_Data ()
        {
            _context.Setup(c => c.Add(It.IsAny<ChurchModel>())).ReturnsAsync(false);
            _context.Setup(c => c.Exists(It.IsAny<ChurchModel>())).ReturnsAsync(false);

            var user = new { Name = "valid_name", Email = "valid_email@email.com", Password = "" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Church.Store, user);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Must_Be_Created_When_the_Data_Is_Correct_and_There_Is_No_Record ()
        {
            _context.Setup(c => c.Add(It.IsAny<ChurchModel>())).ReturnsAsync(true);
            _context.Setup(c => c.Exists(It.IsAny<ChurchModel>())).ReturnsAsync(false);

            var user = new { Name = "valid_name", Email = "valid_email@email.com", Password = "valid_password" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Church.Store, user);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
