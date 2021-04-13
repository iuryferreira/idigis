using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Idigis.Api;
using Idigis.Core.Persistence;
using Idigis.Core.Persistence.Models;
using Idigis.Shared.Dtos.Responses;
using Idigis.Tests.IntegrationTests.Persistence.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.IntegrationTests.Api
{
    [TestClass]
    public class TitheControllerTest
    {
        private Context _context;
        private WebApplicationFactory<Startup> _factory;

        [TestInitialize]
        public void BeforeEach ()
        {
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
        public async Task The_Store_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var data = new
            {
                MemberId = Guid.NewGuid().ToString(),
                ChurchId = Guid.NewGuid().ToString(),
                Value = 1,
                Date = DateTime.Now
            };
            var response = await client.PostAsJsonAsync($"{Routes.Tithe.Base}/", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Store_Method_Must_Return_a_Bad_Request_When_Receiving_Invalid_Data ()
        {
            var data = new { MemberId = Guid.NewGuid().ToString(), ChurchId = Guid.NewGuid().ToString(), Value = 0 };
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync(Routes.Tithe.Base, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Store_Method_Must_Return_Created_When_Receiving_Valid_Data ()
        {
            var church = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(church);
            var member = new MemberModel
            {
                Id = Guid.NewGuid().ToString(), FullName = "any_name", ChurchId = church.Id
            };
            await _context.MemberContext.AddAsync(member);
            await _context.SaveChangesAsync();
            var data = new { ChurchId = church.Id, MemberId = member.Id, Value = 2, Date = DateTime.Now };
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync(Routes.Tithe.Base, data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Tithe.Base}/any_id?churchId=any_id&memberId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Tithe.Base}/any_id?churchId=any_id&memberId=any_id");
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
            var member = new MemberModel
            {
                Id = Guid.NewGuid().ToString(), FullName = "any_name", ChurchId = church.Id
            };
            await _context.MemberContext.AddAsync(member);
            var tithe = new TitheModel
            {
                Id = Guid.NewGuid().ToString(), MemberId = member.Id, Value = 2, Date = DateTime.Now
            };
            await _context.TitheContext.AddAsync(tithe);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            var response =
                await client.GetAsync($"{Routes.Tithe.Base}/{tithe.Id}?churchId={church.Id}&memberId={member.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadFromJsonAsync<GetTitheResponse>();
            Assert.AreEqual(content?.Id, tithe.Id);
            Assert.AreEqual(content?.Value, tithe.Value);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Tithe.Base}?churchId=any_id&memberId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Tithe.Base}?churchId=any_id&memberId=any_id");
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
            var member = new MemberModel
            {
                Id = Guid.NewGuid().ToString(), FullName = "any_name", ChurchId = church.Id
            };
            await _context.MemberContext.AddAsync(member);
            var tithe = new TitheModel
            {
                Id = Guid.NewGuid().ToString(), MemberId = member.Id, Value = 2, Date = DateTime.Now
            };
            await _context.TitheContext.AddAsync(tithe);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Tithe.Base}?churchId={church.Id}&memberId={member.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var data = new { ChurchId = "", MemberId = "", Value = 2, Date = DateTime.Now };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Tithe.Base}/{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Id ()
        {
            var data = new { ChurchId = "", MemberId = "", Value = 2, Date = DateTime.Now };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Tithe.Base}/{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Bad_Request_When_Receiving_Invalid_Data ()
        {
            var church = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "any_name",
                Password = "any_password",
                Email = "email@email.com"
            };
            await _context.ChurchContext.AddAsync(church);
            var member = new MemberModel
            {
                Id = Guid.NewGuid().ToString(), FullName = "any_name", ChurchId = church.Id
            };
            await _context.MemberContext.AddAsync(member);
            var tithe = new TitheModel
            {
                Id = Guid.NewGuid().ToString(), MemberId = member.Id, Value = 2, Date = DateTime.Now
            };
            await _context.TitheContext.AddAsync(tithe);
            await _context.SaveChangesAsync();
            var data = new { ChurchId = church.Id, MemberId = member.Id, Value = 0 };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Tithe.Base}/{tithe.Id}", data);
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
            var member = new MemberModel
            {
                Id = Guid.NewGuid().ToString(), FullName = "any_name", ChurchId = church.Id
            };
            await _context.MemberContext.AddAsync(member);
            var tithe = new TitheModel
            {
                Id = Guid.NewGuid().ToString(), MemberId = member.Id, Value = 2, Date = DateTime.Now
            };
            await _context.TitheContext.AddAsync(tithe);
            await _context.SaveChangesAsync();
            var data = new { ChurchId = church.Id, MemberId = member.Id, Value = 200.3, Date = DateTime.Now };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Tithe.Base}/{tithe.Id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync($"{Routes.Tithe.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync($"{Routes.Tithe.Base}/any_id?churchId=any_id");
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
            var member = new MemberModel
            {
                Id = Guid.NewGuid().ToString(), FullName = "any_name", ChurchId = church.Id
            };
            await _context.MemberContext.AddAsync(member);
            var tithe = new TitheModel
            {
                Id = Guid.NewGuid().ToString(), MemberId = member.Id, Value = 2, Date = DateTime.Now
            };
            await _context.TitheContext.AddAsync(tithe);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            var response =
                await client.DeleteAsync($"{Routes.Tithe.Base}/{tithe.Id}?churchId={church.Id}&memberId={member.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
