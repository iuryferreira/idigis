using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.Contracts
{
    public interface IOfferUseCase
    {
        AbstractNotificator Notificator { get; }
        public Task<CreateOfferResponse> Add (CreateOfferRequest data);
        public Task<GetOfferResponse> Get (GetOfferRequest data);
        public Task<List<GetOfferResponse>> List (ListOfferRequest data);
        public Task<EditOfferResponse> Edit (EditOfferRequest data);
        public Task<DeleteOfferResponse> Delete (DeleteOfferRequest data);
    }
}
