using FluentValidation;
using Idigis.Core.Domain.Helpers;

namespace Idigis.Core.Domain.ValueObjects
{
    internal class Credentials
    {
        internal Credentials (string email, string password)
        {
            Email = email;
            Password = password;
        }

        internal string Email { get; }
        internal string Password { get; set; }
    }

    internal class CredentialsValidator : AbstractValidator<Credentials>
    {
        internal CredentialsValidator ()
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
