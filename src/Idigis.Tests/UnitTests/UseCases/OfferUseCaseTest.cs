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
    public class OfferUseCaseTest
    {
        private readonly Mock<IChurchRepository> _churchRepository;
        private readonly Mock<IOfferRepository> _repository;
        private AbstractNotificator _notificator;
        private IOfferUseCase _sut;

        public OfferUseCaseTest ()
        {
            _repository = new();
            _churchRepository = new();
        }

        [TestInitialize]
        public void BeforeEach ()
        {
            _notificator = new Notificator();
            _sut = new OfferUseCase(_notificator, _repository.Object, _churchRepository.Object);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Church_Is_Not_Found ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync((Church)null);
            Assert.IsNull(await _sut.Add(new("invalid_id", 0)));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Data_Provided_To_Create_Is_Invalid ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            Assert.IsNull(await _sut.Add(new("", 0)));
        }

        [TestMethod]
        public async Task Must_Add_Notifications_if_Data_Provided_To_Create_Is_Invalid ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            await _sut.Add(new("valid_id", 0));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Value - Este campo deve ser maior que 0.");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Add_Method ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.Add(It.IsAny<Offer>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Add(new("valid_id", 1)));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Add_Method ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.Add(It.IsAny<Offer>())).ReturnsAsync(true);
            var data = new CreateOfferRequest("valid_id", 1);
            var response = await _sut.Add(data);
            Assert.IsInstanceOfType(response, typeof(CreateOfferResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.Value, response.Value);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Church_To_Edit_Is_Not_Found ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync((Church)null);
            Assert.IsNull(await _sut.Edit(new("invalid_id", "offer_id", (decimal)32.1)));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Data_Provided_To_Edit_Is_Invalid ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Offer(3));
            Assert.IsNull(await _sut.Edit(new("valid_id", Guid.NewGuid().ToString(), 0)));
        }

        [TestMethod]
        public async Task Must_Add_Notifications_if_Data_Provided_To_Edit_Is_Invalid ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Offer(3));
            await _sut.Edit(new("valid_id", Guid.NewGuid().ToString(), 0));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Value - Este campo deve ser maior que 0.");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Edit_Method ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Offer(3));
            _repository.Setup(repository => repository.Update(It.IsAny<Offer>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Edit(new("valid_id", Guid.NewGuid().ToString(), 30)));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Offer_Is_Not_Found_In_Edit_Method ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync((Offer)null);
            Assert.IsNull(await _sut.Edit(new("valid_id", Guid.NewGuid().ToString(), 30)));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Edit_Method ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name",
                new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Offer(3));
            _repository.Setup(repository => repository.Update(It.IsAny<Offer>())).ReturnsAsync(true);
            var data = new EditOfferRequest("valid_id", Guid.NewGuid().ToString(), 3);
            var response = await _sut.Edit(data);
            Assert.IsInstanceOfType(response, typeof(EditOfferResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.Value, response.Value);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Church_To_Delete_Offer_Is_Not_Found ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync((Church)null);
            Assert.IsNull(await _sut.Delete(new("invalid_id", "offer_id")));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Id_To_Delete_Is_Not_Found ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync((Offer)null);
            Assert.IsNull(await _sut.Delete(new("invalid_id", "invalid_id")));
        }


        [TestMethod]
        public async Task Must_Return_Response_if_Entity_Is_Deleted ()
        {
            _churchRepository.Setup(repository => repository.GetById(It.IsAny<string>())).ReturnsAsync(new Church(
                "valid_id",
                "valid_name", new("valid_email@email.com", "valid_password")));
            _repository.Setup(repository => repository.Remove(It.IsAny<string>())).ReturnsAsync(true);
            Assert.IsNotNull(await _sut.Delete(new("valid_id", "valid_id")));
        }
    }
}
