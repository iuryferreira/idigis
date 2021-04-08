using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IMemberRepository
    {
        Task<bool> Add (string churchId, Member entity);
        Task<Member> GetById (string churchId, string id);
        Task<bool> Update (string churchId, Member entity);
        Task<bool> Remove (string churchId, string id);
    }
}
