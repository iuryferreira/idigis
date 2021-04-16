using System;
using System.Linq;
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
    public class OfferControllerTest
    {
        private WebApplicationFactory<Startup> _factory;
        private string _token;
        private Context _context;

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
        public async Task The_Store_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync($"{Routes.Offer.Base}/",
                new { ChurchId = Guid.NewGuid().ToString(), Value = 1 });
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Store_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PostAsJsonAsync($"{Routes.Offer.Base}/",
                new { ChurchId = Guid.NewGuid().ToString(), Value = 1 });
            Console.WriteLine(client.DefaultRequestHeaders.Authorization.Scheme);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Store_Method_Must_Return_a_Bad_Request_When_Receiving_Invalid_Data ()
        {
            var data = new { ChurchId = "", Value = 0 };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PostAsJsonAsync(Routes.Offer.Base, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Store_Method_Must_Return_Created_When_Receiving_Valid_Data ()
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
            var data = new { ChurchId = model.Id, Value = 2 };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PostAsJsonAsync(Routes.Offer.Base, data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Offer.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Offer.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Offer.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_Ok_When_Receiving_Valid_Data ()
        {
            var church = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(church);
            var offer = new OfferModel { Id = Guid.NewGuid().ToString(), ChurchId = church.Id, Value = 10 };
            await _context.OfferContext.AddAsync(offer);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Offer.Base}/{offer.Id}?churchId={church.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadFromJsonAsync<GetOfferResponse>();
            Assert.AreEqual(content?.Id, offer.Id);
            Assert.AreEqual(content?.Value, offer.Value);
        }


        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Offer.Base}?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Offer.Base}?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Offer.Base}?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_Ok_When_Receiving_Valid_Data ()
        {
            var church = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(church);
            var offer = new OfferModel { Id = Guid.NewGuid().ToString(), ChurchId = church.Id, Value = 10 };
            await _context.OfferContext.AddAsync(offer);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Offer.Base}?churchId={church.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var data = new { ChurchId = Guid.NewGuid().ToString(), Value = 2 };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Offer.Base}/{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var data = new { ChurchId = Guid.NewGuid().ToString(), Value = 2 };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Offer.Base}/{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Id ()
        {
            var data = new { ChurchId = Guid.NewGuid().ToString(), Value = 2 };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Offer.Base}/{Guid.NewGuid()}", data);
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
            var data = new { ChurchId = Guid.NewGuid().ToString(), Value = 0 };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Offer.Base}/{model.Id}", data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_Ok_When_Receiving_Valid_Data ()
        {
            var church = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(church);
            var offer = new OfferModel { Id = Guid.NewGuid().ToString(), ChurchId = church.Id, Value = 10 };
            await _context.OfferContext.AddAsync(offer);
            await _context.SaveChangesAsync();
            var data = new { ChurchId = church.Id, Value = 2 };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Offer.Base}/{offer.Id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync($"{Routes.Offer.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Offer.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Offer.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_NoContent_When_Receiving_Valid_Data ()
        {
            var church = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(church);
            var offer = new OfferModel { Id = Guid.NewGuid().ToString(), ChurchId = church.Id, Value = 10 };
            await _context.OfferContext.AddAsync(offer);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Offer.Base}/{offer.Id}?churchId={church.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
