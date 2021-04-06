using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Application.UseCases;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Dtos.Requests;
using Idigis.Core.Dtos.Responses;
using Idigis.Core.Persistence.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notie;
using Notie.Contracts;

namespace Idigis.Tests.UnitTests.UseCases
{
    [TestClass]
    public class ChurchUseCaseTest
    {
        private readonly CreateChurchRequest _invalidData;
        private readonly Mock<IChurchRepository> _repository;
        private readonly CreateChurchRequest _validData;
        private AbstractNotificator _notificator;
        private IChurchUseCase _sut;


        public ChurchUseCaseTest ()
        {
            _invalidData = new("", "", "");
            _validData = new("valid_name", "valid_email@email.com", "valid_password");
            _repository = new();
        }

        [TestInitialize]
        public void BeforeEach ()
        {
            _notificator = new Notificator();
            _sut = new ChurchUseCase(_notificator, _repository.Object);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Data_Provided_Is_Invalid ()
        {
            Assert.IsNull(await _sut.Add(_invalidData));
        }

        [TestMethod]
        public async Task Must_Add_Notifications_if_Data_Provided_Is_Invalid ()
        {
            await _sut.Add(_invalidData);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(5, messages.Length);
            Assert.AreEqual(messages[0], "Email - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[1], "Email - Forneça um email válido.");
            Assert.AreEqual(messages[2], "Password - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[3], "Password - Este campo deve ser maior que 8 caracteres.");
            Assert.AreEqual(messages[4], "Name - Este campo deve ser informado(a).");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Add(_validData));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(true);
            var response = await _sut.Add(_validData);
            Assert.IsInstanceOfType(response, typeof(CreateChurchResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(_validData.Name, response.Name);
            Assert.AreEqual(_validData.Email, response.Email);
        }
    }
}
