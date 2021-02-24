using FluentValidation;
using Shared.ValueObjects;

namespace Domain.ValueObjects
{
    public class Credentials : ValueObject
    {
        public string Email { get; private set; }
        public string Password { get; internal set; }

        public Credentials (string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    internal class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator ()
        {
            RuleFor(credential => credential.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(credential => credential.Password).NotNull().NotEmpty().MinimumLength(8);
        }
    }
}
