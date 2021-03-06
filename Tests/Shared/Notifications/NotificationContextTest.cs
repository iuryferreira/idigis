using System.Linq;
using Core.Shared.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Shared.Notifications
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
        public void Must_Return_Different_Messages_When_the_Type_Is_Different ()
        {
            Assert.AreNotEqual(Messages.Minimum(8), Messages.Minimum(8, false));
        }
    }
}
