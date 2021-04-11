using System;
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
    public class ChurchRepositoryTest
    {
        private Context _context;
        private AbstractNotificator _notificator;
        private IChurchRepository _sut;

        [TestInitialize]
        public void BeforeEach ()
        {
            _context = TestContextFactory.CreateDbContext();
            _notificator = new Notificator();
            _sut = new ChurchRepository(_notificator, _context);
        }

        [TestMethod]
        public async Task The_Add_Method_Should_Return_False_If_the_Entity_Already_Exists ()
        {
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            await _sut.Add(entity);
            var response = await _sut.Add(entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Este registro já existe, faça o login.");
        }

        [TestMethod]
        public async Task The_Add_Method_Must_Return_False_If_the_Entity_Cannot_Be_Added ()
        {
            await _context.DisposeAsync();
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            var response = await _sut.Add(entity);
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
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            Assert.IsTrue(await _sut.Add(entity));
        }

        [TestMethod]
        public async Task The_Get_Method_Should_Return_Null_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            var responseById = await _sut.GetById(Guid.NewGuid().ToString());
            var responseByEmail = await _sut.GetByEmail("any_email@email.com");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(responseById);
            Assert.IsNull(responseByEmail);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(2, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Registro não encontrado.");
            Assert.AreEqual(messages[1], "Repository - Registro não encontrado.");
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_Null_If_the_Entity_Cannot_Be_Retrieved_From_the_Database ()
        {
            await _context.DisposeAsync();
            var responseById = await _sut.GetById(Guid.NewGuid().ToString());
            var responseByEmail = await _sut.GetByEmail("any_email@email.com");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(responseById);
            Assert.IsNull(responseByEmail);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(2, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Ocorreu um erro na busca.");
            Assert.AreEqual(messages[1], "Repository - Ocorreu um erro na busca.");
        }

        [TestMethod]
        public async Task The_Get_Method_Must_Return_the_Entity_If_It_Is_Retrieved_from_the_Database ()
        {
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            await _sut.Add(entity);
            Assert.IsNotNull(await _sut.GetById(entity.Id));
            Assert.IsNotNull(await _sut.GetByEmail(entity.Credentials.Email));
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            var response = await _sut.Update(entity);
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
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            var response = await _sut.Update(entity);
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
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            var update = new Church(entity.Id, "any_names", new("any_email@email.com", "any_password"));
            await _sut.Add(entity);
            var response = await _sut.Update(update);
            Assert.IsTrue(response);
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Remove("any_id");
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
            var response = await _sut.Remove("any_id");
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
            var entity = new Church("any_name", new("any_email@email.com", "any_password"));
            await _sut.Add(entity);
            var response = await _sut.Remove(entity.Id);
            Assert.IsTrue(response);
        }
    }
}
