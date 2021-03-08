using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Persistence.Contexts;
using Core.Persistence.Models;
using Tests.Database.Factories;

namespace Tests._Core.Persistence.Contexts
{
    [TestClass]
    public class ChurchContextTest : UnitTest
    {
        private ChurchContext _context;

        [TestInitialize]
        public void BeforeEach ()
        {
            _context = ChurchContextFactoryForTests.CreateDbContext();
        }

        [TestMethod]
        public async Task Should_Return_True_If_the_Church_Is_Successfully_Added_to_the_Database ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Testing",
                Email = "Testing@email.com",
                Password = "Password123"
            };
            var saved = await _context.Add(model);
            Assert.IsTrue(saved);
        }

        [TestMethod]
        public async Task Must_Return_True_If_the_Church_Already_Exists_in_the_Database ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Testing",
                Email = "Testing@email.com",
                Password = "Password123"
            };
            await _context.Add(model);
            Assert.IsTrue(await _context.Exists(model));
        }

        [TestMethod]
        public async Task Must_Return_False_If_the_Add_Method_Throws_Exception ()
        {
            var model = new ChurchModel { Id = Guid.NewGuid().ToString(), Name = "Testing", Password = "Password123" };
            Assert.IsFalse(await _context.Add(model));
        }
    }
}
