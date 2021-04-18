using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Hash;
using Idigis.Api;
using Idigis.Core.Persistence;
using Idigis.Core.Persistence.Models;
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
        private readonly Context _context;
        private readonly WebApplicationFactory<Startup> _factory;

        public IndexControllerTest ()
        {
            Env.TraversePath().Load();
            _context = TestContextFactory.CreateDbContext();
            _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<Context>();
                    services.AddScoped(_ => _context);
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

        [TestMethod]
        public async Task The_Signin_Method_Must_Return_a_Internal_Error_When_the_Church_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var data = new { Email = "valid_email@email.com", Password = "valid_password" };
            var response = await client.PostAsJsonAsync($"{Routes.Index.Signin}", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Signin_Method_Must_Return_a_Not_Found_When_Receiving_Church_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            var data = new { Email = "valid_email@email.com", Password = "valid_password" };
            var response = await client.PostAsJsonAsync($"{Routes.Index.Signin}", data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Signin_Method_Must_Return_Unauthorized_When_Receiving_Invalid_Credentials ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = new Hashio().Hash("any_password"),
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            var data = new { Email = "email@email.com", Password = "invalid_password" };
            var response = await client.PostAsJsonAsync($"{Routes.Index.Signin}", data);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Signin_Method_Must_Return_Ok_When_Receiving_Valid_Data ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = new Hashio().Hash("any_password"),
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            var data = new { Email = "email@email.com", Password = "any_password" };
            var response = await client.PostAsJsonAsync($"{Routes.Index.Signin}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Signin_Method_Must_Return_Internal_Error_When_Authenticate_Failed ()
        {
            Env.LoadContents("JwtSecret=#");
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = new Hashio().Hash("any_password"),
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            var data = new { Email = "email@email.com", Password = "any_password" };
            var response = await client.PostAsJsonAsync($"{Routes.Index.Signin}", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
            Env.LoadContents("JwtSecret=randomV23U5i4a1OvVDOwbDAT");
        }
    }
}
