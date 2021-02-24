using Shared.Notifications;

namespace Shared.Handlers
{
    public abstract class Handler
    {
        private readonly NotificationContext _notificationContext;
        public NotificationContext NotificationContext => _notificationContext;

        public Handler (NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;

        }

    }
}
