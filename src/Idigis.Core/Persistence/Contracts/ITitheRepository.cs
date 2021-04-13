using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Notie.Contracts;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface ITitheRepository
    {
        AbstractNotificator Notificator { get; }
        Task<bool> Add (string churchId, string memberId, Tithe entity);
        Task<Tithe> GetById (string churchId, string memberId, string id);
        Task<List<Tithe>> All (string churchId);
        Task<bool> Update (string churchId, string memberId, Tithe entity);
        Task<bool> Remove (string churchId, string memberId, string id);
    }
}
