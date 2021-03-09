using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Core.Persistence.Contexts;
using Core.Persistence.Contracts;
using Core.Persistence.Models;
using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.Persistence.Database.Factories;
using Property = Core.Shared.Type.Property;

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
            ChurchRepository sut = new(_oldContext);
            await sut.Add(_entity);
            await sut.Add(_entity);
            Assert.IsTrue(sut.Notifications.Count > 0);
            Assert.AreEqual("Repository", sut.Notifications.First().Key);
            Assert.AreEqual("Este registro já existe, faça o login.", sut.Notifications.First().Message);
        }

        [TestMethod]
        public async Task If_the_Entity_Is_Valid_and_There_Is_an_Error_in_the_Insertion_in_the_Database ()
        {
            var mockedContext = new Mock<IChurchContext>();
            mockedContext.Setup(c => c.Save()).Throws(new());
            ChurchRepository sut = new(mockedContext.Object);
            await sut.Add(_entity);
            Assert.IsTrue(sut.Notifications.Count > 0);
            Assert.AreEqual("Repository", sut.Notifications.First().Key);
            Assert.AreEqual("Ocorreu um erro na inserção.", sut.Notifications.First().Message);
        }

        [TestMethod]
        public async Task Must_Return_Null_If_the_Entity_Is_Not_Added ()
        {
            ChurchRepository sut = new(_oldContext);
            await sut.Add(_entity);
            Assert.IsFalse(await sut.Add(_entity));
        }
        
        [TestMethod]
        public async Task Must_Return_Null_If_the_Entity_Not_Found ()
        {
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync((ChurchModel)null);
            var entity = new Login("not_found@email.com", "any_password");
            var sut = new ChurchRepository(_context.Object);
            var result = await sut.Get(new (){Key = "Email", Value = entity.Email});
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public async Task Must_Return_Notification_If_the_Entity_Not_Found ()
        {
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync((ChurchModel)null);
            var entity = new Login("not_found@email.com", "any_password");
            var sut = new ChurchRepository(_context.Object);
            var result = await sut.Get(new (){Key = "Email", Value = entity.Email});
            Assert.IsTrue(sut.Notifications.Count > 0);
            Assert.AreEqual("Repository", sut.Notifications.First().Key);
            Assert.AreEqual("Registro não encontrado. Verifique as informações inseridas.", sut.Notifications.First().Message);
        }
        
        [TestMethod]
        public async Task Must_Return_A_Church_If_Found ()
        {
            var login = new Login("found@email.com", "any_password");
            var entity = new Church(Guid.NewGuid().ToString(), "Found", new Credentials(login.Email, login.Password));
            _context.Setup(c => c.Get(It.IsAny<Property>())).ReturnsAsync(entity);
            var sut = new ChurchRepository(_context.Object);
            Assert.IsNotNull(await sut.Get(new (){Key = "Email", Value = login.Email}));
        }
    }
}
