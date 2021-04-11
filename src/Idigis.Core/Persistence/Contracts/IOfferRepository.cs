using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Notie.Contracts;

namespace Idigis.Core.Persistence.Contracts
{
    internal interface IOfferRepository
    {
        AbstractNotificator Notificator { get; }
        Task<bool> Add (string churchId, Offer entity);
        Task<Offer> GetById (string churchId, string id);
        Task<List<Offer>> All (string churchId);
        Task<bool> Update (string churchId, Offer entity);
        Task<bool> Remove (string churchId, string id);
    }
}
