using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Persistence.Contexts;
using Core.Persistence.Contracts;
using Core.Persistence.Models;
using Core.Persistence.Repositories;
using Core.Shared.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.Persistence.Database.Factories;
using Property = Core.Shared.Types.Property;

namespace Tests.Persistence.Repositories
{
    [TestClass]
    public class ChurchRepositoryTest
    {
        private ChurchContext _oldContext;
        private readonly Mock<IChurchContext> _context;
        private Church _entity;

        public ChurchRepositoryTest ()
        {
            _context = new();
        }


        [TestInitialize]
        public void BeforeEach ()
        {
            _oldContext = ChurchContextFactoryForTests.CreateDbContext();
            _entity = new("name", new("", "valid_password"));
        }

        [TestMethod]
        public async Task Must_Notify_If_Church_Already_Exists_in_the_Database ()
        {
            _context.Setup(c => c.Exists(It.IsAny<ChurchModel>())).ReturnsAsync(true);
            var sut = new ChurchRepository(_context.Object, new Notificator());
            var entity = new Church("valid_name", new("found@email.com", "valid_password"));
            await sut.Add(entity);
            Assert.IsTrue(sut.Notificator.HasNotifications);
            Assert.AreEqual("Repository", sut.Notificator.Notifications.First().Key);
            Assert.AreEqual("Este registro já existe, faça o login.", sut.Notificator.Notifications.First().Message);
        }

        [TestMethod]
        public async Task If_the_Entity_Is_Valid_and_There_Is_an_Error_in_the_Insertion_in_the_Database ()
        {
            _context.Setup(c => c.Save()).Throws(new());
            var sut = new ChurchRepository(_context.Object, new Notificator());
            await sut.Add(_entity);
            Assert.IsTrue(sut.Notificator.Notifications.Count > 0);
            Assert.AreEqual("Repository", sut.Notificator.Notifications.First().Key);
            Assert.AreEqual("Ocorreu um erro na inserção.", sut.Notificator.Notifications.First().Message);
        }

        [TestMethod]
        public async Task Must_Return_Null_If_the_Entity_Is_Not_Added ()
        {
            _context.Setup(c => c.Add(It.IsAny<ChurchModel>())).ReturnsAsync(false);
            var sut = new ChurchRepository(_context.Object, new Notificator());
            Assert.IsFalse(await sut.Add(_entity));
        }

        [TestMethod]
        public async Task Must_Return_Null_If_the_Entity_Not_Found ()
        {
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync((ChurchModel)null);
            var entity = new Login("not_found@email.com", "any_password");
            var sut = new ChurchRepository(_context.Object, new Notificator());
            var result = await sut.Get(new() { Key = "Email", Value = entity.Email });
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Must_Return_Notification_If_the_Entity_Not_Found ()
        {
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync((ChurchModel)null);
            var entity = new Login("not_found@email.com", "any_password");
            var sut = new ChurchRepository(_context.Object, new Notificator());
            var result = await sut.Get(new() { Key = "Email", Value = entity.Email });
            Assert.IsTrue(sut.Notificator.Notifications.Count > 0);
            Assert.AreEqual("Repository", sut.Notificator.Notifications.First().Key);
            Assert.AreEqual("Registro não encontrado. Verifique as informações inseridas.", sut.Notificator.Notifications.First().Message);
        }

        [TestMethod]
        public async Task Must_Return_A_Church_If_Found ()
        {
            var login = new Login("found@email.com", "any_password");
            var entity = new Church(Guid.NewGuid().ToString(), "Found", new(login.Email, login.Password));
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync(entity);
            var sut = new ChurchRepository(_context.Object, new Notificator());
            Assert.IsNotNull(await sut.Get(new() { Key = "Email", Value = login.Email }));
        }
    }
}
