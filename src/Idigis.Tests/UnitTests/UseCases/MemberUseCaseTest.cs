using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Application.UseCases;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notie;
using Notie.Contracts;

namespace Idigis.Tests.UnitTests.UseCases
{
    [TestClass]
    public class MemberUseCaseTest
    {
        private readonly Mock<IMemberRepository> _repository;
        private AbstractNotificator _notificator;
        private IMemberUseCase _sut;

        public MemberUseCaseTest ()
        {
            _repository = new();
        }

        [TestInitialize]
        public void BeforeEach ()
        {
            _notificator = new Notificator();
            _sut = new MemberUseCase(_notificator, _repository.Object);
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Add_Notifications_if_Data_Provided_To_Create_Is_Invalid ()
        {
            var response = await _sut.Add(new("valid_id", ""));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "FullName - Este campo deve ser informado(a).");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Add_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<string>(), It.IsAny<Member>())).ReturnsAsync(false);
            Assert.IsNull(await _sut.Add(new("valid_id", "any_name")));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Add_Method ()
        {
            _repository.Setup(repository => repository.Add(It.IsAny<string>(), It.IsAny<Member>())).ReturnsAsync(true);
            var data = new CreateMemberRequest("valid_id", "any_name", new(2000, 10, 10), new(2012, 10, 10),
                new("00000000000", "0", "any_street", "any_district", "any_city"));
            var response = await _sut.Add(data);
            Assert.IsInstanceOfType(response, typeof(CreateMemberResponse));
            Assert.IsNotNull(response.Id);
            Assert.AreEqual(data.FullName, response.FullName);
            Assert.AreEqual(data.BirthDate, response.BirthDate);
            Assert.AreEqual(data.BaptismDate, response.BaptismDate);
            Assert.AreEqual(data.Contact.City, response.Contact.City);
            Assert.AreEqual(data.Contact.District, response.Contact.District);
            Assert.AreEqual(data.Contact.Street, response.Contact.Street);
            Assert.AreEqual(data.Contact.HouseNumber, response.Contact.HouseNumber);
            Assert.AreEqual(data.Contact.PhoneNumber, response.Contact.PhoneNumber);
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Add_Notifications_if_Data_Provided_To_Edit_Is_Invalid ()
        {
            var response = await _sut.Edit(new("valid_id", Guid.NewGuid().ToString(), ""));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "FullName - Este campo deve ser informado(a).");
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Entity_is_Not_Persisted_In_Edit_Method ()
        {
            _repository.Setup(repository => repository.Update(It.IsAny<string>(), It.IsAny<Member>()))
                .ReturnsAsync(false);
            Assert.IsNull(await _sut.Edit(new("valid_id", Guid.NewGuid().ToString(), "valid_name")));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_Is_Valid_And_Persisted_In_Edit_Method ()
        {
            _repository.Setup(repository => repository.Update(It.IsAny<string>(), It.IsAny<Member>()))
                .ReturnsAsync(true);
            var data = new EditMemberRequest("valid_id", Guid.NewGuid().ToString(), "any_name",
                new(2000, 10, 10), new(2012, 10, 10),
                new("00000000000", "0",
                    "any_street", "any_district", "any_city"));
            var response = await _sut.Edit(data);
            Assert.IsInstanceOfType(response, typeof(EditMemberResponse));
            Assert.AreEqual(data.Id, response.Id);
            Assert.AreEqual(data.FullName, response.FullName);
            Assert.AreEqual(data.BirthDate, response.BirthDate);
            Assert.AreEqual(data.BaptismDate, response.BaptismDate);
            Assert.AreEqual(data.Contact.City, response.Contact.City);
            Assert.AreEqual(data.Contact.District, response.Contact.District);
            Assert.AreEqual(data.Contact.Street, response.Contact.Street);
            Assert.AreEqual(data.Contact.HouseNumber, response.Contact.HouseNumber);
            Assert.AreEqual(data.Contact.PhoneNumber, response.Contact.PhoneNumber);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Id_To_Delete_Is_Not_Found ()
        {
            _repository.Setup(repository => repository.Remove(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            Assert.IsNull(await _sut.Delete(new("invalid_id", "invalid_id")));
        }


        [TestMethod]
        public async Task Must_Return_Response_if_Entity_Is_Deleted ()
        {
            _repository.Setup(repository => repository.Remove(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            Assert.IsNotNull(await _sut.Delete(new("valid_id", "valid_id")));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_the_Entity_To_Get_Is_Not_Found ()
        {
            _repository.Setup(repository => repository.GetById(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Member)null);
            Assert.IsNull(await _sut.Get(new("invalid_id", "invalid_id")));
        }

        [TestMethod]
        public async Task Must_Return_a_Response_If_the_Entity_To_Get_Is_Found ()
        {
            _repository.Setup(repository => repository.GetById(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Member("valid_name"));
            Assert.IsNotNull(await _sut.Get(new("valid_id", "valid_id")));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_the_Entity_To_List_Is_Not_Found ()
        {
            _repository.Setup(repository => repository.All(It.IsAny<string>()))
                .ReturnsAsync((List<Member>)null);
            var members = await _sut.List(new("invalid_id"));
            Assert.IsNull(members);
        }

        [TestMethod]
        public async Task Must_Return_an_List_if_the_Entity_To_List_Is_Found ()
        {
            _repository.Setup(repository => repository.All(It.IsAny<string>()))
                .ReturnsAsync(new List<Member> { new("any_name") });
            var members = await _sut.List(new("invalid_id"));
            Assert.AreEqual(1, members.Count);
        }
    }
}
