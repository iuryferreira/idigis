using System.Collections.Generic;
using System.Linq;
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
        private Mock<IChurchRepository> _mockRepository;
        private Mock<IChurchRepository> _mockRepositoryFailed;


        [TestInitialize]
        public void SetUp ()
        {
            _mockRepository = new();
            _mockRepositoryFailed = new();
            _mockRepository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(true);
            _mockRepositoryFailed.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(false);
            _mockRepositoryFailed.SetupGet(repository => repository.Notifications)
                .Returns(new List<Notification> { new("Repository", "") });
        }

        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Invalid ()
        {
            CreateChurchHandler sut = new(new Notificator(), _mockRepository.Object);
            CreateChurchRequest data = new("", "", "");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Should_Return_Response_If_the_Entity_Is_Valid ()
        {
            CreateChurchHandler sut = new(new Notificator(), _mockRepository.Object);
            CreateChurchRequest data = new("valid_name", "valid_email@email.com", "valid_password");
            var result = await sut.Handle(data, new());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateChurchResponse));
        }

        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Not_Saved ()
        {
            CreateChurchHandler sut = new(new Notificator(), _mockRepositoryFailed.Object);
            CreateChurchRequest data = new("valid_name", "valid_email@email.com", "valid_password");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
            Assert.AreEqual("Repository", sut.Notificator.Notifications.First().Key);
        }
    }
}
