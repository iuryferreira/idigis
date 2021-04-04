using System.Collections.Generic;
using FluentValidation;
using Hash;
using Idigis.Core.Domain.Aggregates;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;
using Idigis.Core.Domain.ValueObjects;

namespace Idigis.Core.Domain.Entities
{
    internal class Church : Entity
    {
        private readonly List<Offer> _offers;

        internal Church (string name, Credentials credentials)
        {
            Name = name;
            Credentials = credentials;
            _offers = new();
            if (Validate(this, new ChurchValidator()))
            {
                Credentials.Password = new Hashio().Hash(Credentials.Password);
            }
        }

        private string Name { get; }
        private Credentials Credentials { get; }
        public IReadOnlyCollection<Offer> Offers => _offers;

        internal Offer AddOffer (decimal value)
        {
            var offer = new Offer(value);
            if (!offer.Invalid)
            {
                _offers.Add(offer);
            }

            return offer;
        }

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
