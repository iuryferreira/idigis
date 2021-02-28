using System.Threading.Tasks;
using Application.Handlers;
using Application.Requests;
using Application.Responses;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests._Core.Application.Handlers
{
    [TestClass]
    public class CreateChurchHandlerTest
    {
        private Mock<IChurchRepository> _mockRepository;
        private Church _successEntity;

        [TestInitialize]
        public void SetUp ()
        {
            _successEntity = new("Iury", new("valid_email@email.com", "valid_password"));
            _mockRepository = new();
            _mockRepository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(_successEntity);
        }

        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Invalid ()
        {
            CreateChurchHandler sut = new(new(), _mockRepository.Object);
            CreateChurch data = new("", "", "");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Should_Return_Response_If_the_Entity_Is_Valid ()
        {
            CreateChurchHandler sut = new(new(), _mockRepository.Object);
            CreateChurch data = new("valid_name", "valid_email@email.com", "valid_password");
            var result = await sut.Handle(data, new());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateChurchResponse));
        }
    }
}
