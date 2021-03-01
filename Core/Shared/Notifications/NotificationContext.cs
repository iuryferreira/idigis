using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Shared.Notifications
{
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;

        public NotificationContext ()
        {
            _notifications = new();
        }

        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();

        public void AddNotification (string key, string message)
        {
            _notifications.Add(new(key, message));
        }

        public void AddNotifications (ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddNotification(error.ErrorCode, error.ErrorMessage);
            }
        }

        public void AddNotifications (IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }
    }
}
