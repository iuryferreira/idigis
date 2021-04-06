using System;
using System.Linq;
using Idigis.Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Entities
{
    [TestClass]
    public class TitheTest
    {
        private Tithe _titheValid, _titheInvalid;

        [TestInitialize]
        public void BeforeEach ()
        {
            _titheValid = new(20, DateTime.Now, "valid_id");
            _titheInvalid = new(0, DateTime.Now, "");
        }

        [TestMethod]
        public void Entity_Must_Be_Invalid_if_Receive_a_Invalid_Data_in_Creation ()
        {
            Assert.IsTrue(_titheInvalid.Invalid);
        }

        [TestMethod]
        public void Entity_Should_Return_Error_Messages_if_Receive_a_Invalid_Data_in_Creation ()
        {
            var messages = _titheInvalid.ValidationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")
                .ToArray();
            Assert.AreEqual(2, messages.Length);
            Assert.AreEqual(messages[0], "Value - Este campo deve ser maior que 0.");
            Assert.AreEqual(messages[1], "MemberId - Este campo deve ser informado(a).");
        }

        [TestMethod]
        public void Entity_Must_Be_Valid_if_Receive_a_Valid_Data_in_Creation ()
        {
            Assert.IsFalse(_titheValid.Invalid);
        }
    }
}
