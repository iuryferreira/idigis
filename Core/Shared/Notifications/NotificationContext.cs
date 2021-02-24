using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Notifications
{
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;
        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();

        public NotificationContext ()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification (string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }
        public void AddNotifications (ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddNotification(error.ErrorCode, error.ErrorMessage);
            }
        }
    }
}
