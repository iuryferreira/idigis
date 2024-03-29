using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Idigis.Api;
using Idigis.Api.Auth;
using Idigis.Core.Persistence;
using Idigis.Core.Persistence.Models;
using Idigis.Shared.Dtos.Requests;
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
    public class MemberControllerTest
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly string _token;
        private Context _context;

        public MemberControllerTest ()
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

        [TestInitialize]
        public void BeforeEach ()
        {
            _context = TestContextFactory.CreateDbContext();
        }


        [TestMethod]
        public async Task The_Store_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync($"{Routes.Member.Base}/",
                new { ChurchId = Guid.NewGuid().ToString(), Fullname = "any_name" });
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task The_Store_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PostAsJsonAsync($"{Routes.Member.Base}/",
                new CreateMemberRequest
                {
                    ChurchId = Guid.NewGuid().ToString(), FullName = "any_name", Contact = null
                });
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Store_Method_Must_Return_a_Bad_Request_When_Receiving_Invalid_Data ()
        {
            var data = new { ChurchId = "", Fullname = "" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PostAsJsonAsync(Routes.Member.Base, data);
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
            var data = new CreateMemberRequest { ChurchId = model.Id, FullName = "any_name", Contact = null };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PostAsJsonAsync(Routes.Member.Base, data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Member.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Member.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Member.Base}/any_id?churchId=any_id");
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
                Id = Guid.NewGuid().ToString(), ChurchId = church.Id, FullName = "any_name"
            };
            await _context.MemberContext.AddAsync(member);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Member.Base}/{member.Id}?churchId={church.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadFromJsonAsync<GetMemberResponse>();
            Assert.AreEqual(content?.Id, member.Id);
            Assert.AreEqual(content?.FullName, member.FullName);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"{Routes.Member.Base}?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Member.Base}?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Member.Base}?churchId=any_id");
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
                Id = Guid.NewGuid().ToString(), ChurchId = church.Id, FullName = "any_name"
            };
            await _context.MemberContext.AddAsync(member);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.GetAsync($"{Routes.Member.Base}?churchId={church.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var data = new { ChurchId = Guid.NewGuid().ToString(), FullName = "any_name" };
            var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync($"{Routes.Member.Base}/{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var data = new { ChurchId = Guid.NewGuid().ToString(), FullName = "any_name" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Member.Base}/{Guid.NewGuid()}", data);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Update_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Id ()
        {
            var data = new { ChurchId = Guid.NewGuid().ToString(), FullName = "any_name" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Member.Base}/{Guid.NewGuid()}", data);
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
            var data = new { ChurchId = Guid.NewGuid().ToString(), FullName = "" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Member.Base}/{model.Id}", data);
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
                Id = Guid.NewGuid().ToString(), ChurchId = church.Id, FullName = "any_name"
            };
            await _context.MemberContext.AddAsync(member);
            await _context.SaveChangesAsync();
            var data = new { ChurchId = church.Id, FullName = "any_name" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.PutAsJsonAsync($"{Routes.Member.Base}/{member.Id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Unauthorized_Error_When_the_Request_Not_Have_Valid_Token ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync($"{Routes.Member.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Internal_Error_When_the_Request_Failed ()
        {
            await _context.DisposeAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Member.Base}/any_id?churchId=any_id");
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task The_Delete_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Member.Base}/any_id?churchId=any_id");
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
                Id = Guid.NewGuid().ToString(), ChurchId = church.Id, FullName = "any_name"
            };
            await _context.MemberContext.AddAsync(member);
            await _context.SaveChangesAsync();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", _token);
            var response = await client.DeleteAsync($"{Routes.Member.Base}/{member.Id}?churchId={church.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
