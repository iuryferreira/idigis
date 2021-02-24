using FluentValidation;
using Shared.ValueObjects;

namespace Domain.ValueObjects
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
            RuleFor(credential => credential.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(credential => credential.Password).NotNull().NotEmpty().MinimumLength(8);
        }
    }
}
