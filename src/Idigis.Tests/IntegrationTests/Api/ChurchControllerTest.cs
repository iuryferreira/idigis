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
    public class ChurchControllerTest
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly Context _context;

        public ChurchControllerTest ()
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
        public async Task The_Get_Method_Must_Return_a_Not_Found_When_Receiving_Invalid_Data ()
        {
            var client = _factory.CreateClient();
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
            var response = await client.GetAsync($"{Routes.Church.Base}{model.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadFromJsonAsync<GetChurchResponse>();
            Assert.AreEqual(content?.Id, model.Id);
            Assert.AreEqual(content?.Name, model.Name);
        }
    }
}