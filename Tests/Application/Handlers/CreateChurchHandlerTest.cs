using System.Threading.Tasks;
using Core.Application.Handlers;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Shared.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Application.Handlers
{
    [TestClass]
    public class CreateChurchHandlerTest
    {
        private readonly Mock<IChurchRepository> _repository;

        public CreateChurchHandlerTest ()
        {
            _repository = new();
        }

        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Invalid ()
        {
            CreateChurchHandler sut = new(new Notificator(), _repository.Object);
            CreateChurchRequest data = new("", "", "");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Should_Return_Response_If_the_Entity_Is_Valid ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(true);
            CreateChurchHandler sut = new(new Notificator(), _repository.Object);
            CreateChurchRequest data = new("valid_name", "valid_email@email.com", "valid_password");
            var result = await sut.Handle(data, new());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateChurchResponse));
        }

        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Not_Saved ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(false);
            CreateChurchHandler sut = new(new Notificator(), _repository.Object);
            CreateChurchRequest data = new("valid_name", "valid_email@email.com", "valid_password");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }
    }
}
