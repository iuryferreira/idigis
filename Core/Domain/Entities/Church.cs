using Domain.ValueObjects;
using FluentValidation;
using Hash;
using Shared.Entities;

namespace Domain.Entities
{
    public class Church : Entity
    {
        public Church (string name, Credentials credentials)
        {
            Name = name;
            Credentials = credentials;
            if (Validate(this, new ChurchValidator()))
            {
                Credentials.Password = new Hashio().Hash(Credentials.Password);
            }
        }

        public Church (string id, string name, Credentials credentials)
        {
            Id = id;
            Name = name;
            Credentials = credentials;
        }

        public string Name { get; }

        public Credentials Credentials { get; }
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
