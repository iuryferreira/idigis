using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Shared.Notifications;
using Core.Shared.Type;

namespace Core.Persistence.Contracts
{
    public interface IChurchRepository
    {
        List<Notification> Notifications { get; }
        Task<bool> Add (Church entity);
        Task<Church> Get (Property property);
    }
}
