using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Aggregates;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Domain.Entities
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
            Assert.AreEqual("AspNetCoreCompatibleEmailValidator",
                sut.ValidationResult.Errors.First(error => error.ErrorCode == "AspNetCoreCompatibleEmailValidator")
                    .ErrorCode);
            Assert.AreEqual("NotEmptyValidator",
                sut.ValidationResult.Errors.First(error => error.ErrorCode == "NotEmptyValidator").ErrorCode);
            Assert.AreEqual("MinimumLengthValidator",
                sut.ValidationResult.Errors.First(error => error.ErrorCode == "MinimumLengthValidator").ErrorCode);
        }

        [TestMethod]
        public void Should_Generate_a_Hash_in_the_Entered_Password ()
        {
            Church sut = new("valid_name", new("valid_email@gmail.com", "valid_password"));
            Assert.AreNotEqual("valid_password", sut.Credentials.Password);
            Assert.AreEqual(75, sut.Credentials.Password.Length);
        }

        [TestMethod]
        public void Must_Add_the_Offer_To_the_List ()
        {
            var offer = new Offer((decimal)3.25);
            var sut = new Church("valid_name", new("not_valid_email", "valid_password"));
            sut.AddOffer(offer);

            Assert.AreEqual(1, sut.Offers.Count);
        }

        [TestMethod]
        public void Must_Receive_The_Offers_List_In_Constructor ()
        {
            var offers = new List<Offer>() { new((decimal)3.25), new((decimal)3.2) };
            var sut = new Church(Guid.NewGuid().ToString(), "valid_name", new("not_valid_email", "valid_password"), offers);
            Assert.AreEqual(2, sut.Offers.Count);
        }

    }
}
