using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Application.Responses;
using Domain.Contracts;
using Domain.Entities;
using Shared.Notifications;

namespace Application.Handlers
{
    public class CreateChurchHandler : ICreateChurchHandler
    {
        private readonly IChurchRepository _repository;
        private readonly INotificator _notificator;
        public INotificator Notificator => _notificator;

        public CreateChurchHandler (INotificator notificator, IChurchRepository repository)
        {
            _notificator = notificator;
            _repository = repository;
        }

        public async Task<CreateChurchResponse> Handle (CreateChurch request, CancellationToken cancellationToken)
        {
            Church entity = new(request.Name, new(request.Email, request.Password));
            if (entity.Invalid)
            {
                _notificator.AddNotifications(entity.ValidationResult);
                return null;
            }
            if (!await _repository.Add(entity))
            {
                _notificator.AddNotifications(_repository.Notifications);
                return null;
            }
            var response =
                new CreateChurchResponse { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
            return response;
        }
    }
}
