using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Shared.Notifications
{
    public class Notificator : INotificator
    {
        private readonly List<Notification> _notifications;

        public Notificator ()
        {
            _notifications = new();
        }

        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();

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

        public void AddNotification (string key, string message)
        {
            _notifications.Add(new(key, message));
        }
    }
}
