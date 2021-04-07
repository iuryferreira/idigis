using System.Threading.Tasks;
using Idigis.Core.Dtos.Requests;
using Idigis.Core.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.Contracts
{
    public interface IOfferUseCase
    {
        AbstractNotificator Notificator { get; }
        public Task<CreateOfferResponse> Add (CreateOfferRequest data);
        public Task<EditOfferResponse> Edit (EditOfferRequest data);
        public Task<DeleteOfferResponse> Delete (DeleteOfferRequest data);
    }
}
