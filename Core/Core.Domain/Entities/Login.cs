using Core.Shared.Entities;
using Core.Shared.Notifications;
using FluentValidation;

namespace Core.Domain.Entities
{
    public class Login : Entity
    {
        public Login (string email, string password)
        {
            Email = email;
            Password = password;
            Validate(this, new LoginValidator());
        }

        public string Email { get; }
        public string Password { get; }
    }

    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator ()
        {
            RuleFor(login => login.Email)
                .NotEmpty().WithMessage(Messages.NotEmpty)
                .EmailAddress().WithMessage(Messages.Email);
            RuleFor(credential => credential.Password)
                .NotEmpty().WithMessage(Messages.NotEmpty);
        }
    }
}
