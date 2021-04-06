using System.Linq;
using Idigis.Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Entities
{
    [TestClass]
    public class OfferTest
    {
        private Offer _offerValid, _offerInvalid;

        [TestInitialize]
        public void BeforeEach ()
        {
            _offerValid = new(1);
            _offerInvalid = new(0);
        }

        [TestMethod]
        public void Entity_Must_Be_Invalid_if_Receive_a_Invalid_Data_in_Creation ()
        {
            Assert.IsTrue(_offerInvalid.Invalid);
        }

        [TestMethod]
        public void Entity_Should_Return_Error_Messages_if_Receive_a_Invalid_Data_in_Creation ()
        {
            var messages = _offerInvalid.ValidationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")
                .ToArray();
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual(messages[0], "Value - Este campo deve ser maior que 0.");
        }

        [TestMethod]
        public void Entity_Must_Be_Valid_if_Receive_a_Valid_Data_in_Creation ()
        {
            Assert.IsFalse(_offerValid.Invalid);
        }
    }
}
