using System.Threading;
using System.Threading.Tasks;
using Core.Application.Contracts;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Shared.Notifications;

namespace Core.Application.Handlers
{
    public class CreateChurchHandler : ICreateChurchHandler
    {
        private readonly IChurchRepository _repository;

        public CreateChurchHandler (INotificator notificator, IChurchRepository repository)
        {
            Notificator = notificator;
            _repository = repository;
        }

        private INotificator Notificator { get; }

        public async Task<CreateChurchResponse> Handle (CreateChurchRequest request,
            CancellationToken cancellationToken)
        {
            Church entity = new(request.Name, new(request.Email, request.Password));
            if (entity.Invalid)
            {
                Notificator.AddNotifications(entity.ValidationResult);
                return null;
            }
            
            if (!await _repository.Add(entity))
            {
                return null;
            }

            var response =
                new CreateChurchResponse { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
            return response;
        }
    }
}
