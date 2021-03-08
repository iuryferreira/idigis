using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Shared.Notifications;

namespace Core.Persistence.Contracts
{
    public interface IChurchRepository
    {
        List<Notification> Notifications { get; }
        Task<bool> Add (Church entity);
    }
}
