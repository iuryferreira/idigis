using System.Linq;
using Idigis.Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Entities
{
    [TestClass]
    public class ChurchTest
    {
        private Church _churchValid, _churchInvalid;

        [TestInitialize]
        public void BeforeEach ()
        {
            _churchValid = new("valid_name", new("valid@email.com", "valid_password"));
            _churchInvalid = new("", new("", ""));
        }

        [TestMethod]
        public void Entity_Must_Be_Invalid_if_Receive_a_Invalid_Data_in_Creation ()
        {
            Assert.IsTrue(_churchInvalid.Invalid);
        }

        [TestMethod]
        public void Entity_Should_Return_Error_Messages_if_Receive_a_Invalid_Data_in_Creation ()
        {
            var messages = _churchInvalid.ValidationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")
                .ToArray();
            Assert.AreEqual(5, messages.Length);
            Assert.AreEqual(messages[0], "Credentials.Email - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[1], "Credentials.Email - Forneça um email válido.");
            Assert.AreEqual(messages[2], "Credentials.Password - Este campo deve ser informado(a).");
            Assert.AreEqual(messages[3], "Credentials.Password - Este campo deve ser maior que 8 caracteres.");
            Assert.AreEqual(messages[4], "Name - Este campo deve ser informado(a).");
        }

        [TestMethod]
        public void Entity_Must_Be_Valid_if_Receive_a_Valid_Data_in_Creation ()
        {
            Church sut = new("valid_name", new("valid@email.com", "valid_password"));
            Assert.IsFalse(_churchValid.Invalid);
        }
    }
}
