using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IMemberRepository
    {
        Task<bool> Add (Member entity);
        Task<Member> GetById (string id);
        Task<bool> Update (Member entity);
        Task<bool> Remove (string id);
    }
}
