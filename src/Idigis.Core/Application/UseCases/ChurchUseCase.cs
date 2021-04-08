using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Dtos.Requests;
using Idigis.Core.Dtos.Responses;
using Idigis.Core.Persistence.Contracts;
using Notie.Contracts;

namespace Idigis.Core.Application.UseCases
{
    internal class ChurchUseCase : IChurchUseCase
    {
        private readonly IChurchRepository _repository;

        public ChurchUseCase (AbstractNotificator notificator, IChurchRepository repository)
        {
            Notificator = notificator;
            _repository = repository;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<CreateChurchResponse> Add (CreateChurchRequest data)
        {
            var entity = new Church(data.Name, new(data.Email, data.Password));
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Add(entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
        }

        public async Task<EditChurchResponse> Edit (EditChurchRequest data)
        {
            var entity = new Church(data.Id, data.Name, new(data.Email, data.Password));
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Update(entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
        }

        public async Task<DeleteChurchResponse> Delete (DeleteChurchRequest data)
        {
            if (!await _repository.Remove(data.Id))
            {
                return null;
            }

            return new();
        }
    }
}
