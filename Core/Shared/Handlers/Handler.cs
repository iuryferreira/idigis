using Shared.Notifications;

namespace Shared.Handlers
{
    public abstract class Handler : Notifiable
    {
        protected Handler (NotificationContext notificationContext) : base(notificationContext) { }
    }
}
