using System.Linq;
using Idigis.Core.Domain.Aggregates;
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
        public void Entity_Should_Return_Error_Messages_if_Receive_a_Invalid_Data_in_Creation ()
        {
            Church sut = new("", new("", ""));
            var messages = sut.ValidationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}").ToArray();
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
            Assert.IsFalse(sut.Invalid);
        }

        [TestMethod]
        public void Must_Return_an_Offer_When_Receiving_the_Data ()
        {
            Church sut = new("valid_name", new("valid@email.com", "valid_password"));
            var offer = sut.AddOffer(0);
            Assert.IsInstanceOfType(offer, typeof(Offer));
        }

        [TestMethod]
        public void Must_Not_Add_an_Offer_When_It_Is_Invalid ()
        {
            Church sut = new("valid_name", new("valid@email.com", "valid_password"));
            sut.AddOffer(0);
            Assert.AreEqual(0, sut.Offers.Count);
        }

        [TestMethod]
        public void Must_Add_an_Offer_When_It_Is_Valid ()
        {
            Church sut = new("valid_name", new("valid@email.com", "valid_password"));
            sut.AddOffer(1);
            Assert.AreEqual(1, sut.Offers.Count);
        }
    }
}
