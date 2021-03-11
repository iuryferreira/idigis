using Core.Application.Requests.OfferRequests;
using Core.Application.Responses.OfferResponses;
using MediatR;

namespace Core.Application.Contracts
{
    public interface ICreateOfferHandler : IRequestHandler<CreateOfferRequest, CreateOfferResponse>
    {
    }
}
