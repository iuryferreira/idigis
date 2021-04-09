using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Dtos.Requests;
using Idigis.Core.Dtos.Responses;
using Idigis.Core.Persistence.Contracts;
using Notie.Contracts;

namespace Idigis.Core.Application.UseCases
{
    internal class OfferUseCase : IOfferUseCase
    {
        private readonly IOfferRepository _repository;


        public OfferUseCase (AbstractNotificator notificator, IOfferRepository repository)
        {
            _repository = repository;
            Notificator = notificator;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<CreateOfferResponse> Add (CreateOfferRequest data)
        {
            var entity = new Offer(data.Value);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Add(data.ChurchId, entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Value = entity.Value };
        }

        public async Task<GetOfferResponse> Get (GetOfferRequest data)
        {
            var offer = await _repository.GetById(data.ChurchId, data.Id);
            return offer is null ? null : new() { Id = offer.Id, Value = offer.Value };
        }

        public async Task<List<GetOfferResponse>> List (ListOfferRequest data)
        {
            var offers = await _repository.All(data.ChurchId);
            return offers is null
                ? new()
                : offers.Select(offer => new GetOfferResponse { Id = offer.Id, Value = offer.Value }).ToList();
        }

        public async Task<EditOfferResponse> Edit (EditOfferRequest data)
        {
            var entity = new Offer(data.Id, data.Value);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Update(data.ChurchId, entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Value = entity.Value };
        }

        public async Task<DeleteOfferResponse> Delete (DeleteOfferRequest data)
        {
            if (!await _repository.Remove(data.ChurchId, data.Id))
            {
                return null;
            }

            return new();
        }
    }
}
