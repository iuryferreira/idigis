using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Database.Factories;

namespace Tests._Core.Persistence.Factories
{
    [TestClass]
    public class ChurchContextFactoryTest
    {
        [TestMethod]
        public void An_Exception_Must_Be_Returned_If_the_Context_Cannot_Be_Created ()
        {
            Exception exception = null;
            try
            {
                ChurchContextFactoryForTests.CreateDbContext("invalid");
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsNotNull(exception);
            Assert.AreEqual("Could not connect to the database.", exception.Message);
        }
    }
}
