using System.Threading.Tasks;
using Application.Handlers;
using Application.Requests;
using Application.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests._Core.Application.Handlers
{
    [TestClass]
    public class CreateChurchHandlerTest
    {
        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Invalid ()
        {
            CreateChurchHandler sut = new(new());
            CreateChurch data = new("", "", "");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Should_Return_Response_If_the_Entity_Is_Valid ()
        {
            CreateChurchHandler sut = new(new());
            CreateChurch data = new("valid_name", "valid_email@email.com", "valid_password");
            var result = await sut.Handle(data, new());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateChurchResponse));
        }
    }
}
