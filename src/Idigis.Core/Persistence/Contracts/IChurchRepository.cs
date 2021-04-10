using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Notie.Contracts;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IChurchRepository
    {
        AbstractNotificator Notificator { get; }
        Task<bool> Add (Church entity);
        Task<Church> GetById (string id);
        Task<Church> GetByEmail (string email);
        Task<bool> Update (Church entity);
        Task<bool> Remove (string id);
    }
}
