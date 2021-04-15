using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
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
                Notificator.SetNotificationType(new("Validation"));
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Add(entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
        }

        public async Task<GetChurchResponse> Get (GetChurchRequest data)
        {
            var church = string.IsNullOrEmpty(data.Id)
                ? await _repository.GetByEmail(data.Email)
                : await _repository.GetById(data.Id);
            if (church is null)
            {
                return null;
            }

            return new()
            {
                Id = church.Id,
                Name = church.Name,
                Email = church.Credentials.Email,
                Password = church.Credentials.Password
            };
        }

        public async Task<EditChurchResponse> Edit (EditChurchRequest data)
        {
            var entity = new Church(data.Id, data.Name, new(data.Email, data.Password));
            if (entity.Invalid)
            {
                Notificator.SetNotificationType(new("Validation"));
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
