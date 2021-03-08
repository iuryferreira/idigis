using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests._Core.Domain.Entities
{
    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void Should_Returns_False_if_Receive_a_Invalid_Id ()
        {
            Church sut = new("invalid_id", "", new("", ""));
            ChurchValidator validator = new();
            var result = validator.Validate(sut);
            Assert.IsFalse(result.IsValid);
        }
    }
}
