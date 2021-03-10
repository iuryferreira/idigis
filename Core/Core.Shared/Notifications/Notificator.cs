using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Core.Shared.Notifications
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
        public NotificationType NotificationType { get; set; } = NotificationType.Internal;

        public void AddNotifications (ValidationResult validationResult)
        {
            NotificationType = NotificationType.Validation;
            
            foreach (var error in validationResult.Errors)
            {
                AddNotification(error.PropertyName.Substring(error.PropertyName.IndexOf('.') + 1), error.ErrorMessage);
            }
        }
        
        public void AddNotification (string key, string message)
        {
            _notifications.Add(new(key, message));
        }
    }
}
