using Core.Application.Responses.OfferResponses;
using MediatR;

namespace Core.Application.Requests.OfferRequests
{
    public class CreateOfferRequest : IRequest<CreateOfferResponse>
    {
        public string UserId { get; init; }
        public decimal Value { get; init; }

        public CreateOfferRequest (string userId, decimal value)
        {
            Value = value;
            UserId = userId;
        }
    }
}
