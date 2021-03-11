using System.Linq;
using Core.Domain.Aggregates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Domain.Entities
{
    [TestClass]
    public class OfferTest
    {

        [TestMethod]
        public void Should_Returns_False_if_Receive_a_Invalid_Data ()
        {
            var sut = new Offer(0);
            Assert.IsFalse(sut.Valid);
        }

        [TestMethod]
        public void Should_Returns_Message_if_Receive_a_Invalid_Data ()
        {
            var sut = new Offer(0);
            Assert.AreEqual("Este campo deve ser maior que 0.", sut.ValidationResult.Errors.FirstOrDefault().ErrorMessage);
            Assert.AreEqual("GreaterThanValidator", sut.ValidationResult.Errors.FirstOrDefault().ErrorCode);
        }
    }
}
