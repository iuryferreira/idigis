using System;
using System.Threading.Tasks;
using Core.Persistence.Contexts;
using Core.Persistence.Models;
using Core.Shared.Type;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Persistence.Database.Factories;

namespace Tests.Persistence.Contexts
{
    [TestClass]
    public class ChurchContextTest
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
        
        [TestMethod]
        public async Task Must_Return_ChurchModel_Instance_If_the_Church_Already_Exists_in_the_Database ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Testing",
                Email = "testing@email.com",
                Password = "Password123"
            };
            await _context.Add(model);
            var property = new Property(){Key = "Email", Value = model.Email};
            var result = await _context.Get(property);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ChurchModel));
        }
        
        [TestMethod]
        public async Task Must_Return_Null_If_the_Church_Not_Exists_in_The_Database ()
        {
            var model = new ChurchModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Testing",
                Email = "testing@email.com",
                Password = "Password123"
            };
            await _context.Add(model);
            var property = new Property(){Key = "Email", Value = "invalid"};
            var result = await _context.Get(property);
            Assert.IsNull(result);
        }
    }
}
