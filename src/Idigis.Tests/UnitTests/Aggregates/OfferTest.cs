using Idigis.Core.Domain.Aggregates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Aggregates
{
    [TestClass]
    public class OfferTest
    {
        [TestMethod]
        public void Entity_Must_Be_Invalid_if_Receive_a_Invalid_Data_in_Creation ()
        {
            var sut = new Offer(0);
            Assert.IsTrue(sut.Invalid);
        }

        [TestMethod]
        public void Entity_Must_Be_Valid_if_Receive_a_Valid_Data_in_Creation ()
        {
            var sut = new Offer(3);
            Assert.IsFalse(sut.Invalid);
        }
    }
}
