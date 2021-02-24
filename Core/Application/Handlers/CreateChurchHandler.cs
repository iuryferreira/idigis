using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Application.Responses;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Shared.Notifications;

namespace Application.Handlers
{
    public class CreateChurchHandler : IRequestHandler<CreateChurch, CreateChurchResponse>
    {
        private readonly NotificationContext _notificationContext;
        private readonly IChurchRepository _repository;

        public CreateChurchHandler (NotificationContext notificationContext, IChurchRepository repository)
        {
            _notificationContext = notificationContext;
            _repository = repository;
        }

        public Task<CreateChurchResponse> Handle (CreateChurch request, CancellationToken cancellationToken)
        {
            Church entity = new(request.Name, new(request.Email, request.Password));
            if (entity.Invalid)
            {
                _notificationContext.AddNotifications(entity.ValidationResult);
                return Task.FromResult<CreateChurchResponse>(null);
            }

            var response = new CreateChurchResponse { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
            return Task.FromResult(response);
        }
    }
}
