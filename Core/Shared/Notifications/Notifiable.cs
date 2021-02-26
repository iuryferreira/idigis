namespace Shared.Notifications
{
    public abstract class Notifiable
    {
        private readonly NotificationContext _notificationContext;
        public NotificationContext NotificationContext => _notificationContext;

        public Notifiable (NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;

        }

    }
}
