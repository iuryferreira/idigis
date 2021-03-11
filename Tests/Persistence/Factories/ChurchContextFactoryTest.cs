using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Persistence.Database.Factories;

namespace Tests.Persistence.Factories
{
    [TestClass]
    public class ChurchContextFactoryTest
    {
        [TestMethod]
        public void An_Exception_Must_Be_Returned_If_the_Context_Cannot_Be_Created ()
        {
            var exception =
                Assert.ThrowsException<Exception>(() => ChurchContextFactoryForTests.CreateDbContext("invalid"));
            Assert.AreEqual("Could not connect to the database.", exception.Message);
        }
    }
}
