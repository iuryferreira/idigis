using Application.Responses;
using MediatR;

namespace Application.Requests
{
    public class CreateChurch : IRequest<CreateChurchResponse>
    {
        public CreateChurch (string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
