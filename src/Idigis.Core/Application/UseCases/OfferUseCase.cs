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
