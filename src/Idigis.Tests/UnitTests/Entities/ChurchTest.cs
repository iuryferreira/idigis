using Idigis.Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Entities
{
    [TestClass]
    public class ChurchTest
    {
        [TestMethod]
        public void Entity_Must_Be_Invalid_if_Receive_a_Invalid_Data_in_Creation ()
        {
            Church sut = new("", new("", ""));
            Assert.IsTrue(sut.Invalid);
        }

        [TestMethod]
        public void Entity_Must_Be_Valid_if_Receive_a_Valid_Data_in_Creation ()
        {
            Church sut = new("valid_name", new("valid@email.com", "valid_password"));
            Assert.IsFalse(sut.Invalid);
        }
    }
}
