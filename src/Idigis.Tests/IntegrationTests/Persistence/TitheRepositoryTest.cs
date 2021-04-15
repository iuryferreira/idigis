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
    public class TitheRepositoryTest
    {
        private Church _church;
        private IChurchRepository _churchRepository;
        private Context _context;
        private Member _member;
        private IMemberRepository _memberRepository;
        private AbstractNotificator _notificator;
        private ITitheRepository _sut;

        [TestInitialize]
        public void BeforeEach ()
        {
            _context = TestContextFactory.CreateDbContext();
            _notificator = new Notificator();
            _sut = new TitheRepository(_notificator, _context);
            _churchRepository = new ChurchRepository(_notificator, _context);
            _memberRepository = new MemberRepository(_notificator, _context);
            _church = new("any_name", new("any_email@email.com", "any_password"));
            _member = new("any_name");
            _churchRepository.Add(_church);
            _memberRepository.Add(_church.Id, _member);
        }

        [TestMethod]
        public async Task The_Add_Method_Should_Return_False_If_the_Church_Not_Exists ()
        {
            var entity = new Tithe(3, DateTime.Now);
            var response = await _sut.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Add_Method_Should_Return_False_If_the_Member_Not_Exists ()
        {
            var entity = new Tithe(3, DateTime.Now);
            var response = await _sut.Add(_church.Id, Guid.NewGuid().ToString(), entity);
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }


        [TestMethod]
        public async Task The_Add_Method_Must_Return_False_If_the_Entity_Cannot_Be_Added ()
        {
            await _context.DisposeAsync();
            var entity = new Tithe(3, DateTime.Now);
            var response = await _sut.Add(_church.Id, _member.Id, entity);
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
            var entity = new Tithe(3, DateTime.Now);
            Assert.IsTrue(await _sut.Add(_church.Id, _member.Id, entity));
        }

        [TestMethod]
        public async Task The_Get_Method_Should_Return_Null_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.GetById("any_id", "any_id", "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Get_Method_Should_Return_Null_If_the_Member_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.GetById(_church.Id, "any_id", "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Get_Method_Should_Return_Null_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.GetById(_church.Id, _member.Id, "any_id");
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
            var response = await _sut.GetById(_church.Id, _member.Id, "any_id");
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
            var entity = new Tithe(3, DateTime.Now);
            await _sut.Add(_church.Id, _member.Id, entity);
            Assert.IsNotNull(await _sut.GetById(_church.Id, _member.Id, entity.Id));
        }

        [TestMethod]
        public async Task The_All_Method_Should_Return_Null_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.All("any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsNull(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja não existe no sistema.");
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
        public async Task The_All_Method_Must_Return_the_List_of_Entities_If_It_Is_Retrieved_from_the_Database ()
        {
            Assert.IsInstanceOfType(await _sut.All(_church.Id), typeof(List<Tithe>));
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Update("any_id", "any_id", new(3, DateTime.Now));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Member_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Update(_member.Id, "any_id", new(3, DateTime.Now));
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Update_Method_Should_Return_False_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var entity = new Tithe(3, DateTime.Now);
            var response = await _sut.Update(_church.Id, _member.Id, entity);
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
            var entity = new Tithe(3, DateTime.Now);
            var response = await _sut.Update(_church.Id, _member.Id, entity);
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
            var entity = new Tithe(3, DateTime.Now);
            await _sut.Add(_church.Id, _member.Id, entity);
            var response = await _sut.Update(_church.Id, _member.Id, new(entity.Id, 5, DateTime.Now));
            Assert.IsTrue(response);
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Church_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Remove("any_id", "any_id", "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Member_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Remove(_church.Id, "any_id", "any_id");
            var messages = _sut.Notificator.Notifications
                .Select(notification => $"{notification.Key} - {notification.Message}").ToArray();
            Assert.IsFalse(response);
            Assert.IsTrue(_sut.Notificator.HasNotifications);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Repository - Esta igreja ou membro não existe no sistema.");
        }

        [TestMethod]
        public async Task The_Remove_Method_Should_Return_False_If_the_Entity_Does_Not_Exist_in_the_Database ()
        {
            var response = await _sut.Remove(_church.Id, _member.Id, "any_id");
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
            var response = await _sut.Remove(_church.Id, _member.Id, "any_id");
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
            var entity = new Tithe(3, DateTime.Now);
            await _sut.Add(_church.Id, _member.Id, entity);
            var response = await _sut.Remove(_church.Id, _member.Id, entity.Id);
            Assert.IsTrue(response);
        }
    }
}
