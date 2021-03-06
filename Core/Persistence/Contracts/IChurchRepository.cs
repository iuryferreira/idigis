using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Shared.Notifications;

namespace Persistence.Contracts
{
    public interface IChurchRepository
    {
        List<Notification> Notifications { get; }
        Task<bool> Add (Church entity);
    }
}
