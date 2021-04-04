using FluentValidation;
using Hash;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;
using Idigis.Core.Domain.ValueObjects;

namespace Idigis.Core.Domain.Entities
{
    internal class Church : Entity
    {
        internal Church (string name, Credentials credentials)
        {
            Name = name;
            Credentials = credentials;
            if (Validate(this, new ChurchValidator()))
            {
                Credentials.Password = new Hashio().Hash(Credentials.Password);
            }
        }

        private string Name { get; }
        private Credentials Credentials { get; }

        private class ChurchValidator : AbstractValidator<Church>
        {
            public ChurchValidator ()
            {
                Include(new EntityValidator());
                RuleFor(church => church.Credentials).SetValidator(new CredentialsValidator());
                RuleFor(church => church.Name).NotEmpty().WithMessage(Messages.NotEmpty);
            }
        }
    }
}