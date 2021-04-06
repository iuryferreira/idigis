using System.Linq;
using Idigis.Core.Domain.Aggregates;
using Idigis.Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Idigis.Tests.UnitTests.Aggregates
{
    [TestClass]
    public class OfferTest
    {
        private Church _churchValid;
        private Offer _offerValid, _offerInvalid;

        [TestInitialize]
        public void BeforeEach ()
        {
            _offerValid = new(1);
            _offerInvalid = new(0);
            _churchValid = new("valid_name", new("valid@email.com", "valid_password"));
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

        [TestMethod]
        public void Must_Return_an_Offer_When_Receiving_the_Data ()
        {
            var offer = _churchValid.AddOffer(0);
            Assert.IsInstanceOfType(offer, typeof(Offer));
        }

        [TestMethod]
        public void Must_Not_Add_an_Offer_When_It_Is_Invalid ()
        {
            _churchValid.AddOffer(0);
            Assert.AreEqual(0, _churchValid.Offers.Count);
        }

        [TestMethod]
        public void Must_Add_an_Offer_When_It_Is_Valid ()
        {
            _churchValid.AddOffer(1);
            Assert.AreEqual(1, _churchValid.Offers.Count);
        }

        [TestMethod]
        public void Must_Return_Null_When_the_Offer_To_Be_Removed_Does_Not_Exist ()
        {
            var offer = _churchValid.RemoveOffer("invalid_id");
            Assert.IsNull(offer);
        }

        [TestMethod]
        public void Must_Return_Offer_When_the_Offer_To_Be_Removed_Exists ()
        {
            _churchValid.AddOffer(1);
            var offer = _churchValid.AddOffer(1);
            Assert.IsNotNull(_churchValid.RemoveOffer(offer.Id));
        }

        [TestMethod]
        public void Must_Return_the_Sum_of_the_Values_of_the_Offers ()
        {
            _churchValid.AddOffer(3);
            _churchValid.AddOffer(4);
            Assert.AreEqual(7, _churchValid.TotalOffers);
        }
    }
}
