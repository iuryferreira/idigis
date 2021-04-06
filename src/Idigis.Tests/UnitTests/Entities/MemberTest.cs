using System;
using System.Linq;
using Idigis.Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Entities
{
    [TestClass]
    public class MemberTest
    {
        private Member _memberValid, _memberInvalid;

        [TestInitialize]
        public void BeforeEach ()
        {
            _memberValid = new(
                "Valid Fullname",
                new(1990, 03, 10),
                new(1990, 03, 10),
                new("00000000000",
                    "123A",
                    "valid_street",
                    "valid_district",
                    "valid_city")
            );
            _memberInvalid = new("", DateTime.Now.AddYears(1), DateTime.Now.AddYears(1));
        }

        [TestMethod]
        public void Entity_Must_Be_Invalid_if_Receive_a_Invalid_Data_in_Creation ()
        {
            Assert.IsTrue(_memberInvalid.Invalid);
        }

        [TestMethod]
        public void Entity_Should_Return_Error_Messages_if_Receive_a_Invalid_Data_in_Creation ()
        {
            var messages = _memberInvalid.ValidationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")
                .ToArray();
            Assert.AreEqual(3, messages.Length);
            Assert.AreEqual(messages[0], "FullName - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[1], "BirthDate - Esta data não é valida.");
            Assert.AreEqual(messages[2], "BaptismDate - Esta data não é valida.");
        }

        [TestMethod]
        public void Entity_Must_Be_Valid_if_Receive_a_Valid_Data_in_Creation ()
        {
            Assert.IsFalse(_memberValid.Invalid);
        }
    }
}
