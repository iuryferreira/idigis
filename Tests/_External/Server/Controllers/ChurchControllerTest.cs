using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Contracts;

namespace Tests._External.Server.Controllers
{
    [TestClass]
    public class ChurchControllerTest : IntegrationTest
    {
        [TestMethod]
        public async Task Must_Return_a_Bad_Request_When_Trying_to_Register_a_Church_with_Wrong_Data ()
        {
            var user = new CreateChurch("valid_name", "valid_email@email.com", "");
            var response = await Client.PostAsJsonAsync(ApiRoutes.Church.Store, user);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Must_Be_Created_When_the_Data_Is_Correct_and_There_Is_No_Record ()
        {
            var user = new CreateChurch("valid_name", "valid_email@email.com", "valid_password");
            var response = await Client.PostAsJsonAsync(ApiRoutes.Church.Store, user);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
