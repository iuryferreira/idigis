using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IOfferRepository
    {
        Task<bool> Add (string churchId, Offer entity);
        Task<Offer> GetById (string churchId, string id);
        Task<bool> Update (string churchId, Offer entity);
        Task<bool> Remove (string churchId, string id);
    }
}
