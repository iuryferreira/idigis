using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Persistence.Contracts;
using Core.Persistence.Models;
using Core.Shared.Types;
using External.Server;
using External.Server.Contracts;
using Hash;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Server.Controllers
{
    [TestClass]
    public class ChurchControllerTest
    {
        private readonly Mock<IChurchContext> _context;
        private readonly WebApplicationFactory<Startup> _factory;

        public ChurchControllerTest ()
        {
            _context = new();
            _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IChurchContext>();
                    services.AddScoped(service => _context.Object);
                });
            });
        }

        [TestMethod]
        public async Task Must_Return_a_Bad_Request_When_Trying_to_Register_a_Church_with_Wrong_Data ()
        {
            _context.Setup(c => c.Add(It.IsAny<ChurchModel>())).ReturnsAsync(false);
            _context.Setup(c => c.Exists(It.IsAny<ChurchModel>())).ReturnsAsync(false);
            var data = new { Name = "valid_name", Email = "valid_email@email.com", Password = "" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Index.Signup, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Must_Be_Created_When_the_Data_Is_Correct_and_There_Is_No_Record ()
        {
            _context.Setup(c => c.Add(It.IsAny<ChurchModel>())).ReturnsAsync(true);
            _context.Setup(c => c.Exists(It.IsAny<ChurchModel>())).ReturnsAsync(false);
            var data = new { Name = "valid_name", Email = "valid_email@email.com", Password = "valid_password" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Index.Signup, data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Should_Return_NotFound_If_the_User_Is_Not_Found ()
        {
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync((ChurchModel)null);
            _context.Setup(c => c.Exists(It.IsAny<ChurchModel>())).ReturnsAsync(false);
            var data = new { Email = "valid_email@email.com", Password = "valid_password" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Index.Signin, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Should_Return_Unauthorized_If_the_Credentials_Is_Invalid ()
        {
            var model = new ChurchModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Valid Name",
                Email = "found@email.com",
                Password = new Hashio().Hash("any_password")
            };
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync(model);
            var data = new { Email = "found@email.com", Password = "valid_password" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Index.Signin, data);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Must_Return_a_Bad_Request_When_Inserting_Invalid_Data ()
        {
            var data = new { Email = "", Password = "" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Index.Signin, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Must_Return_a_Ok_Request_When_Inserting_Valid_Data ()
        {
            var model = new ChurchModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Valid Name",
                Email = "found@email.com",
                Password = new Hashio().Hash("valid_password")
            };
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync(model);
            var data = new { Email = "found@email.com", Password = "valid_password" };
            var response = await _factory.CreateClient().PostAsJsonAsync(ApiRoutes.Index.Signin, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
