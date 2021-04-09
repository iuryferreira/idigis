using System;
using System.Collections.Generic;
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
    public class TitheUseCaseTest
    {
        private readonly Mock<ITitheRepository> _repository;

        private AbstractNotificator _notificator;
        private ITitheUseCase _sut;

        public TitheUseCaseTest ()
        {
            _repository = new();
        }

        [TestInitialize]
        public void BeforeEach ()

        {
            _notificator = new Notificator();
            _sut = new TitheUseCase(_notificator, _repository.Object);
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Notifications_if_Data_Provided_To_Create_Is_Invalid ()
        {
            var response = await _sut.Add(new("valid_id", "valid_id", 0, DateTime.Now.AddYears(1)));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Value - Este campo deve ser maior que 0.");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Add_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Tithe>()))
                .ReturnsAsync(false);
            Assert.IsNull(await _sut.Add(new("valid_id", "valid_id", 10, DateTime.Now.AddYears(1))));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Add_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Tithe>()))
                .ReturnsAsync(true);
            var data = new CreateTitheRequest("valid_id", "valid_id", 10, DateTime.Now);
            var response = await _sut.Add(data);
            Assert.IsInstanceOfType(response, typeof(CreateTitheResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.Value, response.Value);
            Assert.AreEqual(data.Date, response.Date);
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Notifications_if_Data_Provided_To_Edit_Is_Invalid ()
        {
            var response = await _sut.Edit(new("valid_id", "valid_id", "valid_id", 0, DateTime.Now.AddYears(1)));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(2, messages.Length);
            Assert.AreEqual(messages[0],
                "Id - 'Id' deve ser maior ou igual a 36 caracteres. Você digitou 8 caracteres.");
            Assert.AreEqual(messages[1], "Value - Este campo deve ser maior que 0.");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Edit_Method ()
        {
            _repository.Setup(
                    repository => repository.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Tithe>()))
                .ReturnsAsync(false);
            Assert.IsNull(await _sut.Edit(new("valid_id", "valid_id", Guid.NewGuid().ToString(), 1,
                DateTime.Now.AddYears(1))));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Edit_Method ()
        {
            _repository.Setup(
                    repository => repository.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Tithe>()))
                .ReturnsAsync(true);
            var data = new EditTitheRequest("valid_id", "valid_id", Guid.NewGuid().ToString(), 3, DateTime.Now);
            var response = await _sut.Edit(data);
            Assert.IsInstanceOfType(response, typeof(EditTitheResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.Value, response.Value);
            Assert.AreEqual(data.Date, response.Date);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Id_To_Delete_Is_Not_Found ()
        {
            _repository.Setup(repository =>
                repository.Remove(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Delete(new("invalid_id", "invalid_id", Guid.NewGuid().ToString())));
        }


        [TestMethod]
        public async Task Must_Return_Response_if_Entity_Is_Deleted ()
        {
            _repository.Setup(repository =>
                repository.Remove(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            Assert.IsNotNull(await _sut.Delete(new("invalid_id", "invalid_id", Guid.NewGuid().ToString())));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_the_Entity_To_Get_Is_Not_Found ()
        {
            _repository.Setup(repository =>
                    repository.GetById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Tithe)null);
            Assert.IsNull(await _sut.Get(new("invalid_id", "invalid_id", "invalid_id")));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_To_Get_Is_Found ()
        {
            _repository.Setup(repository =>
                    repository.GetById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Tithe(10, DateTime.Now));
            Assert.IsNotNull(await _sut.Get(new("valid_id", "valid_id", "valid_id")));
        }

        [TestMethod]
        public async Task Must_Return_Empty_List_if_the_Entity_To_List_Is_Not_Found ()
        {
            _repository.Setup(repository => repository.All(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((List<Tithe>)null);
            var members = await _sut.List(new("valid_id", "valid_id"));
            Assert.AreEqual(0, members.Count);
        }

        [TestMethod]
        public async Task Must_Return_an_List_if_the_Entity_To_List_Is_Found ()
        {
            _repository.Setup(repository => repository.All(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<Tithe> { new(1, DateTime.Now) });
            var members = await _sut.List(new("valid_id", "valid_id"));
            Assert.AreEqual(1, members.Count);
        }
    }
}
