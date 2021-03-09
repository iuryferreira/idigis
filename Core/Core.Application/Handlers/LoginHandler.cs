using System.Threading;
using System.Threading.Tasks;
using Core.Application.Contracts;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Authentication.Contracts;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Shared.Notifications;

namespace Core.Application.Handlers
{
    public class LoginHandler : ILoginHandler
    {
        private readonly IJwtService _jwtService;
        private readonly IChurchRepository _repository;

        public LoginHandler (INotificator notificator, IChurchRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
            Notificator = notificator;
        }

        public INotificator Notificator { get; }

        public async Task<LoginResponse> Handle (LoginRequest request, CancellationToken cancellationToken)
        {
            var login = new Login(request.Email, request.Password);
            if (login.Invalid)
            {
                Notificator.AddNotifications(login.ValidationResult);
                return null;
            }

            var user = await _repository.Get(login);
            if (user is not null)
            {
                return new()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Credentials.Email,
                    Token = _jwtService.GenerateToken(user.Credentials.Email, user.Id)
                };
            }

            Notificator.AddNotifications(_repository.Notifications);
            return null;
        }
    }
}
