using Hash;
using FluentValidation;
using Domain.ValueObjects;
using Shared.Entities;

namespace Domain.Entities
{
    internal class Church : Entity
    {
        public string Name { get; private set; }
        public Credentials Credentials { get; private set; }

        public Church (string name, Credentials credentials)
        {
            Name = name;
            Credentials = credentials;

            Validate(this, new ChurchValidator());

            Credentials.Password = new Hashio().Hash(Credentials.Password);
        }

        public Church (string id, string name, Credentials credentials)
        {
            Id = id;
            Name = name;
            Credentials = credentials;

            Validate(this, new ChurchValidator());
        }
    }
    internal class ChurchValidator : AbstractValidator<Church>
    {
        public ChurchValidator ()
        {
            Include(new EntityValidator());
            RuleFor(church => church.Credentials).SetValidator(new CredentialsValidator());
            RuleFor(church => church.Name).NotEmpty().NotNull();
        }
    }
}
