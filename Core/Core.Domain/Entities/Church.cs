using System.Collections.Generic;
using Core.Domain.ValueObjects;
using Core.Shared.Entities;
using Core.Domain.Aggregates;
using Core.Shared.Notifications;
using FluentValidation;
using Hash;

namespace Core.Domain.Entities
{
    public class Church : Entity
    {
        private readonly List<Offer> _offers;

        public Church (string name, Credentials credentials)
        {
            Name = name;
            Credentials = credentials;
            _offers = new List<Offer>();
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

        public Church (string id, string name, Credentials credentials, List<Offer> offers)
        {
            Id = id;
            Name = name;
            Credentials = credentials;
            _offers = offers;
        }

        public void AddOffer (Offer offer)
        {
            _offers.Add(offer);
        }

        public string Name { get; }

        public Credentials Credentials { get; }

        public IReadOnlyCollection<Offer> Offers => _offers;
    }

    internal class ChurchValidator : AbstractValidator<Church>
    {
        public ChurchValidator ()
        {
            Include(new EntityValidator());
            RuleFor(church => church.Credentials).SetValidator(new CredentialsValidator());
            RuleFor(church => church.Name).NotEmpty().WithMessage(Messages.NotEmpty);
        }
    }
}
