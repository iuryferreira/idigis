using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Shared.Notifications;
using Core.Shared.Types;

namespace Core.Persistence.Contracts
{
    public interface IChurchRepository
    {
        INotificator Notificator { get; }
        Task<bool> Add (Church entity);
        Task<bool> Update (Church entity);
        Task<Church> Get (Property property);
    }
}
