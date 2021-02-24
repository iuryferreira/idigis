using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Application.Responses;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Shared.Handlers;
using Shared.Notifications;

namespace Application.Handlers
{
    public class CreateChurchHandler : Handler, IRequestHandler<CreateChurch, CreateChurchResponse>
    {
        private readonly IChurchRepository _repository;

        public CreateChurchHandler (NotificationContext notificationContext, IChurchRepository repository) : base(notificationContext)
        {
            _repository = repository;
        }

        public async Task<CreateChurchResponse> Handle (CreateChurch request, CancellationToken cancellationToken)
        {
            Church entity = new(request.Name, new(request.Email, request.Password));
            if (entity.Invalid)
            {
                NotificationContext.AddNotifications(entity.ValidationResult);
                return null;
            }
            await this._repository.Add(entity);
            var response = new CreateChurchResponse { Id = entity.Id, Name = entity.Name, Email = entity.Credentials.Email };
            return response;
        }
    }
}
