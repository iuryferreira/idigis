using System.Threading;
using System.Threading.Tasks;
using Core.Application.Contracts;
using Core.Application.Requests.OfferRequests;
using Core.Application.Responses.OfferResponses;
using Core.Domain.Aggregates;
using Core.Persistence.Contracts;
using Core.Shared.Notifications;

namespace Core.Application.Handlers.OfferHandlers
{
    public class CreateOfferHandler : ICreateOfferHandler
    {
        private readonly IChurchRepository _repository;

        public CreateOfferHandler (IChurchRepository repository, INotificator notificator)
        {
            Notificator = notificator;
            _repository = repository;
        }

        private INotificator Notificator { get; }

        public async Task<CreateOfferResponse> Handle (CreateOfferRequest request, CancellationToken cancellationToken)
        {
            var church = await _repository.Get(new() { Key = "Id", Value = request.UserId });

            if (church is null)
            {
                return null;
            }

            Offer offer = new(request.Value);

            if (offer.Invalid)
            {
                Notificator.AddNotifications(offer.ValidationResult);
                return null;
            }
            church.AddOffer(offer);

            if (!await _repository.Update(church))
            {
                return null;
            }

            return new()
            {
                Id = offer.Id,
                Value = offer.Value
            };
        }
    }
}
