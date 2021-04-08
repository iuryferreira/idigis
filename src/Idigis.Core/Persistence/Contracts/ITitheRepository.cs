using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface ITitheRepository
    {
        Task<bool> Add (string churchId, string memberId, Tithe entity);
        Task<Tithe> GetById (string churchId, string memberId, string id);
        Task<bool> Update (string churchId, string memberId, Tithe entity);
        Task<bool> Remove (string churchId, string memberId, string id);
    }
}