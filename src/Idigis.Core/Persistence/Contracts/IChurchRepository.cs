using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IChurchRepository
    {
        Task<bool> Add (Church entity);
        Task<Church> GetById (string id);
        Task<bool> Update (Church entity);
        Task<bool> Remove (string id);
    }
}
