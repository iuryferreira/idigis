using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence;
using Idigis.Core.Persistence.Contracts;
using Idigis.Core.Persistence.Repositories;
using Idigis.Tests.IntegrationTests.Persistence.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notie;
using Notie.Contracts;

namespace Idigis.Tests.IntegrationTests.Persistence
{
    [TestClass]
    public class OfferRepositoryTest
    {
        private Church _church;
        private IChurchRepository _churchRepository;
        private Context _context;
        private AbstractNotificator _notificator;
        private IOfferRepository _sut;

        [TestInitialize]
        public void BeforeEach ()
        {
            _context = TestContextFactory.CreateDbContext();
            _notificator = new Notificator();
            _sut = new OfferRepository(_notificator, _context);
            _churchRepository = new ChurchRepository(_notificator, _context);
            _church = new("any_name", new("any_email@email.com", "any_password"));
            _churchRepository.Add(_church);
        }

        [TestMethod]
        public async Task The_Add_Method_Should_Return_False_If_the_Church_Not_Exists ()
        {
            var entity = new Offer(3);
            var response = await _sut.Add(Guid.NewGuid().ToString(), entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Add_Method_Must_Return_False_If_the_Entity_Cannot_Be_Added ()
        {
            await _context.DisposeAsync();
            var entity = new Offer(3);
            var response = await _sut.Add(Guid.NewGuid().ToString(), entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Ocorreu um erro na inserção.");
        }

        [TestMethod]
        public async Task The_Add_Method_Must_Return_True_If_the_Entity_Is_Added ()
        {
            var entity = new Offer(3);
            Assert.IsTrue(await _sut.Add(_church.Id, entity));
        }

        [TestMethod]
        public async Task The_Get_Method_Should_Return_Null_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.GetById(Guid.NewGuid().ToString(), "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Get_Method_Should_Return_Null_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.GetById(_church.Id, "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Registro não encontrado.");
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_Null_If_the_Entity_Cannot_Be_Retrieved_From_the_Database ()
        {
            await _context.DisposeAsync();
            var response = await _sut.GetById(_church.Id, "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Ocorreu um erro na busca.");
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_the_Entity_If_It_Is_Retrieved_from_the_Database ()
        {
            var entity = new Offer(3);
            await _sut.Add(_church.Id, entity);
            Assert.IsNotNull(await _sut.GetById(_church.Id, entity.Id));
        }

        [TestMethod]
        public async Task The_All_Method_Should_Return_Null_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.All(Guid.NewGuid().ToString());
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja não existe no sistema.");
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_the_List_of_Entities_If_It_Is_Retrieved_from_the_Database ()
        {
            Assert.IsInstanceOfType(await _sut.All(_church.Id), typeof(List<Offer>));
        }

        [TestMethod]
        public async Task The_All_Method_Must_Return_Null_If_the_Entities_Cannot_Be_Retrieved_From_the_Database ()
        {
            await _context.DisposeAsync();
            var response = await _sut.All(_church.Id);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Ocorreu um erro na listagem.");
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Update(Guid.NewGuid().ToString(), new(3));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var entity = new Offer(3);
            var response = await _sut.Update(_church.Id, entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Registro não encontrado.");
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Entity_Cannot_Be_Updated ()
        {
            await _context.DisposeAsync();
            var entity = new Offer(3);
            var response = await _sut.Update(_church.Id, entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Ocorreu um erro na atualização.");
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_True_If_the_Entity_Is_Updated ()
        {
            var entity = new Offer(3);
            await _sut.Add(_church.Id, entity);
            var response = await _sut.Update(_church.Id, new(entity.Id, 5));
            Assert.IsTrue(response);
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Remove(Guid.NewGuid().ToString(), "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Remove(_church.Id, "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Registro não encontrado.");
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Entity_Cannot_Be_Removed ()
        {
            await _context.DisposeAsync();
            var response = await _sut.Remove(_church.Id, "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Ocorreu um erro na remoção.");
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_True_If_the_Entity_Is_Updated ()
        {
            var entity = new Offer(3);
            await _sut.Add(_church.Id, entity);
            var response = await _sut.Remove(_church.Id, entity.Id);
            Assert.IsTrue(response);
        }
    }
}
