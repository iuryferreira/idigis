using System.Linq;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests._Core.Domain.Entities
{
    [TestClass]
    public class ChurchTest
    {
        [TestMethod]
        public void Should_Returns_False_if_Receive_a_Invalid_Data ()
        {
            Church sut = new("", "", new("", ""));
            Assert.IsTrue(sut.Invalid);
        }

        [TestMethod]
        public void Should_Generate_an_Id_When_Only_the_Name_And_Credentials_Is_Given ()
        {
            Church sut = new("valid_name", new("", ""));
            Assert.IsNotNull(sut.Id);
        }

        [TestMethod]
        public void Should_Returns_False_if_Receive_a_Invalid_Credentials_Data ()
        {
            Church sut = new("valid_name", new("not_valid_email", ""));
            Assert.IsFalse(sut.Valid);
            Assert.AreEqual(3, sut.ValidationResult.Errors.Count);
            Assert.AreEqual("AspNetCoreCompatibleEmailValidator", (sut.ValidationResult.Errors
                .Where(error => error.ErrorCode == "AspNetCoreCompatibleEmailValidator").First()).ErrorCode);
            Assert.AreEqual("NotEmptyValidator", (sut.ValidationResult.Errors
                .Where(error => error.ErrorCode == "NotEmptyValidator").First()).ErrorCode);
            Assert.AreEqual("MinimumLengthValidator", (sut.ValidationResult.Errors
                .Where(error => error.ErrorCode == "MinimumLengthValidator").First()).ErrorCode);
        }
        [TestMethod]
        public void Should_Generate_a_Hash_in_the_Entered_Password ()
        {
            Church sut = new("valid_name", new("valid_email@gmail.com", "valid_password"));
            Assert.AreNotEqual("valid_password", sut.Credentials.Password);
            Assert.AreEqual(75, sut.Credentials.Password.Length);
        }
    }
}
