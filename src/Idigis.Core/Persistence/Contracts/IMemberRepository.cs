using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Notie.Contracts;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IMemberRepository
    {
        AbstractNotificator Notificator { get; }
        Task<bool> Add (string churchId, Member entity);
        Task<Member> GetById (string churchId, string id);
        Task<List<Member>> All (string churchId);
        Task<bool> Update (string churchId, Member entity);
        Task<bool> Remove (string churchId, string id);
    }
}
