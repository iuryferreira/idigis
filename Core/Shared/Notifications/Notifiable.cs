namespace Shared.Notifications
{
    public abstract class Notifiable
    {
        protected Notifiable (NotificationContext notificationContext)
        {
            NotificationContext = notificationContext;
        }

        public NotificationContext NotificationContext { get; }
    }
}
