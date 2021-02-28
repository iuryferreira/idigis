namespace Shared.Notifications
{
    public abstract class Notifiable
    {
        protected Notifiable (NotificationContext notificationContext)
        {
            NotificationContext = notificationContext;
        }

        protected NotificationContext NotificationContext { get; }
    }
}
