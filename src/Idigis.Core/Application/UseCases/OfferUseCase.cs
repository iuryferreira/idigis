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
        private readonly IChurchRepository _churchRepository;
        private readonly IOfferRepository _repository;


        public OfferUseCase (AbstractNotificator notificator, IOfferRepository repository,
            IChurchRepository churchRepository)
        {
            _repository = repository;
            _churchRepository = churchRepository;
            Notificator = notificator;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<CreateOfferResponse> Add (CreateOfferRequest data)
        {
            var church = await _churchRepository.GetById(data.ChurchId);
            if (church is null)
            {
                return null;
            }

            var entity = new Offer(data.Value);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Add(entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Value = entity.Value };
        }

        public async Task<EditOfferResponse> Edit (EditOfferRequest data)
        {
            var church = await _churchRepository.GetById(data.ChurchId);
            if (church is null)
            {
                return null;
            }

            var offer = await _repository.GetById(data.Id);
            if (offer is null)
            {
                return null;
            }

            var entity = new Offer(data.Id, data.Value);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Update(entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Value = entity.Value };
        }

        public async Task<DeleteOfferResponse> Delete (DeleteOfferRequest data)
        {
            var church = await _churchRepository.GetById(data.ChurchId);
            if (church is null)
            {
                return null;
            }

            if (!await _repository.Remove(data.Id))
            {
                return null;
            }

            return new();
        }
    }
}
