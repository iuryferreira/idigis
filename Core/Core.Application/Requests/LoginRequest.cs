using Core.Application.Responses;
using MediatR;

namespace Core.Application.Requests
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public LoginRequest (string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; init; }
        public string Password { get; init; }
    }
}
