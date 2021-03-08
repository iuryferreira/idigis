using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Core.Persistence.Contexts;
using Core.Persistence.Contracts;
using Core.Persistence.Repositories;
using Tests.Database.Factories;

namespace Tests._Core.Persistence.Repositories
{
    [TestClass]
    public class ChurchRepositoryTest : UnitTest
    {
        private ChurchContext _context;
        private Church _entity;


        [TestInitialize]
        public void BeforeEach ()
        {
            _context = ChurchContextFactoryForTests.CreateDbContext();
            _entity = new("name", new("", "valid_password"));
        }

        [TestMethod]
        public async Task Must_Notify_If_Church_Already_Exists_in_the_Database ()
        {
            ChurchRepository sut = new(_context);
            await sut.Add(_entity);
            await sut.Add(_entity);
            Assert.IsTrue(sut.Notifications.Count > 0);
            Assert.AreEqual("Repository", sut.Notifications.First().Key);
            Assert.AreEqual("O usuário já existe, faça o login.", sut.Notifications.First().Message);
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
            ChurchRepository sut = new(_context);
            await sut.Add(_entity);
            Assert.IsFalse(await sut.Add(_entity));
        }
    }
}
