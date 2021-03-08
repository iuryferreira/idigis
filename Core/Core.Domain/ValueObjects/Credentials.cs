using FluentValidation;
using Shared.Notifications;
using Shared.ValueObjects;

namespace Core.Domain.ValueObjects
{
    public class Credentials : ValueObject
    {
        public Credentials (string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; internal set; }
    }

    internal class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator ()
        {
            RuleFor(credential => credential.Email)
                .NotEmpty().WithMessage(Messages.NotEmpty)
                .EmailAddress().WithMessage(Messages.Email);
            RuleFor(credential => credential.Password)
                .NotEmpty().WithMessage(Messages.NotEmpty)
                .MinimumLength(8).WithMessage(Messages.Minimum(8));
        }
    }
}
