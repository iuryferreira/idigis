using Core.Application.Responses;
using MediatR;

namespace Core.Application.Requests
{
    public class CreateChurchRequest : IRequest<CreateChurchResponse>
    {
        public CreateChurchRequest (string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
