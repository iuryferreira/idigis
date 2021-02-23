using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests._Core.Domain.Entities
{
    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void Should_Returns_False_if_Receive_a_Valid_Id ()
        {
            Church c = new("invalid_id", "valid_name");
            ChurchValidator validator = new();
            var result = validator.Validate(c);
            Assert.IsFalse(result.IsValid);
        }
    }
}
