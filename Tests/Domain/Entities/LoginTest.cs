using System.Linq;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Domain.Entities
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void Should_Returns_False_And_Notifications_If_Receive_a_Invalid_Data ()
        {
            Login sut = new("invalid_email", "");
            LoginValidator validator = new();
            var result = validator.Validate(sut);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, sut.ValidationResult.Errors.Count);
            Assert.AreEqual("AspNetCoreCompatibleEmailValidator",
                sut.ValidationResult.Errors.First(error => error.ErrorCode == "AspNetCoreCompatibleEmailValidator")
                    .ErrorCode);
            Assert.AreEqual("NotEmptyValidator",
                sut.ValidationResult.Errors.First(error => error.ErrorCode == "NotEmptyValidator").ErrorCode);
        }
    }
}
