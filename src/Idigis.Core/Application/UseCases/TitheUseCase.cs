using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.UseCases
{
    internal class TitheUseCase : ITitheUseCase
    {
        private readonly ITitheRepository _repository;

        public TitheUseCase (AbstractNotificator notificator, ITitheRepository repository)
        {
            Notificator = notificator;
            _repository = repository;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<CreateTitheResponse> Add (CreateTitheRequest data)
        {
            var entity = new Tithe(data.Value, data.Date);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Add(data.ChurchId, data.MemberId, entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Value = entity.Value, Date = entity.Date };
        }

        public async Task<GetTitheResponse> Get (GetTitheRequest data)
        {
            var tithe = await _repository.GetById(data.ChurchId, data.MemberId, data.Id);
            return tithe is null ? null : new() { Id = tithe.Id, Value = tithe.Value, Date = tithe.Date };
        }

        public async Task<List<GetTitheResponse>> List (ListTitheRequest data)
        {
            var tithes = await _repository.All(data.ChurchId, data.MemberId);
            return tithes is null
                ? new()
                : tithes.Select(tithe => new GetTitheResponse { Id = tithe.Id, Value = tithe.Value, Date = tithe.Date })
                    .ToList();
        }

        public async Task<EditTitheResponse> Edit (EditTitheRequest data)
        {
            var entity = new Tithe(data.Id, data.Value, data.Date);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Update(data.ChurchId, data.MemberId, entity))
            {
                return null;
            }

            return new() { Id = entity.Id, Value = entity.Value, Date = entity.Date };
        }

        public async Task<DeleteTitheResponse> Delete (DeleteTitheRequest data)
        {
            if (!await _repository.Remove(data.ChurchId, data.MemberId, data.Id))
            {
                return null;
            }

            return new();
        }
    }
}
