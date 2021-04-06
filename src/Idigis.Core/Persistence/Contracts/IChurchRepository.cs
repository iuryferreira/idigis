using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IChurchRepository
    {
        Task<bool> Add (Church entity);
    }
}
