using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Notifications;

namespace Tests._Core.Shared.Notifications
{
    [TestClass]
    public class NotificationContextTest
    {
        [TestMethod]
        public void Should_Return_True_If_There_Is_Any_Notification ()
        {
            Notificator sut = new();
            sut.AddNotification("valid_key", "valid_message");
            Assert.IsTrue(sut.HasNotifications);
        }

        [TestMethod]
        public void Should_Return_a_List_of_Notifications_Greater_than_Zero ()
        {
            Notificator sut = new();
            sut.AddNotification("valid_key", "valid_message");
            Assert.AreEqual(1, sut.Notifications.Count);
        }

        [TestMethod]
        public void Should_Return_a_Valid_Notification ()
        {
            Notificator sut = new();
            sut.AddNotification("valid_key", "valid_message");
            Assert.AreEqual("valid_key", sut.Notifications.Single().Key);
            Assert.AreEqual("valid_message", sut.Notifications.Single().Message);
        }

        [TestMethod]
        public void Should_Allow_Concatenating_Two_Notification_Contexts ()
        {
            Notificator firstContext = new();
            Notificator secondContext = new();
            firstContext.AddNotification("valid_key", "valid_message");
            secondContext.AddNotification("second_valid_key", "second_valid_message");
            firstContext.AddNotifications(secondContext.Notifications.ToList());
            Assert.AreEqual(2, firstContext.Notifications.Count);
        }
    }
}
