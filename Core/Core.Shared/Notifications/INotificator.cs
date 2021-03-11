using System.Collections.Generic;
using FluentValidation.Results;

namespace Core.Shared.Notifications
{
    public interface INotificator
    {
        IReadOnlyCollection<Notification> Notifications { get; }
        bool HasNotifications { get; }
        NotificationType NotificationType { get; set; }
        void AddNotification (string key, string message);
        void AddNotifications (ValidationResult validationResult);
    }
}
