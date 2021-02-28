using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence.Contexts;
using Persistence.Factories;
using Persistence.Models;

namespace Tests._Core.Persistence.Factories
{
    [TestClass]
    public class ChurchContextTest
    {
        private ChurchContext _context;

        [TestInitialize]
        public void BeforeEach ()
        {
            _context = ChurchContextFactory.CreateDbContext();
            _context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void AfterEach ()
        {
            _context.Database.RollbackTransaction();
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
            var saved = await _context.Add(model);
            Assert.IsTrue(await _context.Exists(model));
        }
    }
}
