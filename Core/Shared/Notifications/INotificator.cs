using System.Collections.Generic;
using FluentValidation.Results;

namespace Shared.Notifications
{
    public interface INotificator
    {
        IReadOnlyCollection<Notification> Notifications { get; }
        bool HasNotifications { get; }
        void AddNotifications (ValidationResult validationResult);
        void AddNotifications (IEnumerable<Notification> notifications);
    }
}
