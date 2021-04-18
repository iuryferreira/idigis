using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Idigis.Api;
using Idigis.Api.Auth;
using Idigis.Core.Persistence;
using Idigis.Core.Persistence.Models;
using Idigis.Shared.Dtos.Responses;
using Idigis.Tests.IntegrationTests.Persistence.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notie;

namespace Idigis.Tests.IntegrationTests.Api
{
    [TestClass]
    public class ChurchControllerTest
    {
        private Context _context;
        private WebApplicationFactory<Startup> _factory;
        private string _token;

        [TestInitialize]
        public void BeforeEach ()
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
            _token = new AuthService(new Notificator()).GenerateToken("any_email@email.com", "any_id");
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Church.Base}any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Church.Base}any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Church.Base}any_id");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_Ok_When_Receiving_Valid_Data ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Church.Base}{model.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadFromJsonAsync<GetChurchResponse>();
            Assert.AreEqual(content?.Id, model.Id);
            Assert.AreEqual(content?.Name, model.Name);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var data = new { Name = "other_name", Email = "any_email@email.com", Password = "any_password" };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Church.Base}{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var data = new { Name = "other_name", Email = "any_email@email.com", Password = "any_password" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Church.Base}{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Id ()
        {
            var data = new { Name = "other_name", Email = "any_email@email.com", Password = "any_password" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Church.Base}{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Bad_Request_When_Receiving_Invalid_Data ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var data = new { Name = "other_name", Email = "invalid_email", Password = "any_password" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Church.Base}{model.Id}", data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_Ok_When_Receiving_Valid_Data ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var data = new { Name = "other_name", Email = "valid_email@email.com", Password = "any_password" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Church.Base}{model.Id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync($"{Routes.Church.Base}any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Church.Base}any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Church.Base}any_id");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_NoContent_When_Receiving_Valid_Data ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(model);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Church.Base}{model.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
