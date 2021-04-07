using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IOfferRepository
    {
        Task<bool> Add (Offer entity);
        Task<Offer> GetById (string id);
        Task<bool> Update (Offer entity);
        Task<bool> Remove (string id);
    }
}
