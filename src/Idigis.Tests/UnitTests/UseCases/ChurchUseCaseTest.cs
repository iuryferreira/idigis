using System;
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
        private readonly Mock<IChurchRepository> _repository;
        private AbstractNotificator _notificator;
        private IChurchUseCase _sut;

        public ChurchUseCaseTest ()
        {
            _repository = new();
        }

        [TestInitialize]
        public void BeforeEach ()
        {
            _notificator = new Notificator();
            _sut = new ChurchUseCase(_notificator, _repository.Object);
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Notifications_if_Data_Provided_To_Create_Is_Invalid ()
        {
            var response = await _sut.Add(new("", "", ""));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(5, messages.Length);
            Assert.AreEqual(messages[0], "Email - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[1], "Email - Forneça um email válido.");
            Assert.AreEqual(messages[2], "Password - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[3], "Password - Este campo deve ser maior que 8 caracteres.");
            Assert.AreEqual(messages[4], "Name - Este campo deve ser informado(a).");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Add_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Add(new("valid_name", "valid_email@email.com", "valid_password")));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Add_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(true);
            var data = new CreateChurchRequest("valid_name", "valid_email@email.com", "valid_password");
            var response = await _sut.Add(data);
            Assert.IsInstanceOfType(response, typeof(CreateChurchResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.Name, response.Name);
            Assert.AreEqual(data.Email, response.Email);
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Notifications_if_Data_Provided_To_Edit_Is_Invalid ()
        {
            var response = await _sut.Edit(new("", "", "", ""));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(7, messages.Length);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Edit_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<Church>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Edit(new(Guid.NewGuid().ToString(), "valid_name", "valid_email@email.com",
                "valid_password")));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Edit_Method ()
        {
            _repository.Setup(repository => repository.Update(It.IsAny<Church>())).ReturnsAsync(true);
            var data = new EditChurchRequest(Guid.NewGuid().ToString(), "valid_name", "valid_email@email.com",
                "valid_password");
            var response = await _sut.Edit(data);
            Assert.IsInstanceOfType(response, typeof(EditChurchResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.Name, response.Name);
            Assert.AreEqual(data.Email, response.Email);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Id_To_Delete_Is_Not_Found ()
        {
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync((Church)null);
            Assert.IsNull(await _sut.Delete(new("")));
        }


        [TestMethod]
        public async Task Must_Return_Response_if_Entity_Is_Deleted ()
        {
            _repository.Setup(repository => repository.Remove(It.IsAny<string>())).ReturnsAsync(true);
            Assert.IsNotNull(await _sut.Delete(new("valid_id")));
        }
    }
}
