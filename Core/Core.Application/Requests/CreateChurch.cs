using Core.Application.Responses;
using MediatR;

namespace Core.Application.Requests
{
    public class CreateChurch : IRequest<CreateChurchResponse>
    {
        public CreateChurch (string name, string email, string password)
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
