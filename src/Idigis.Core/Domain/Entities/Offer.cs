using FluentValidation;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;

namespace Idigis.Core.Domain.Entities
{
    internal class Offer : Entity
    {
        internal Offer (decimal value)
        {
            Value = value;
            Validate(this, new OfferValidator());
        }

        internal Offer (string id, decimal value)
        {
            Id = id;
            Value = value;
            Validate(this, new OfferValidator());
        }

        internal decimal Value { get; }

        private class OfferValidator : AbstractValidator<Offer>
        {
            internal OfferValidator ()
            {
                Include(new EntityValidator());
                RuleFor(o => o.Value).NotNull()
                    .WithMessage(Messages.NotEmpty)
                    .GreaterThan(0)
                    .WithMessage(Messages.Minimum(0, false));
            }
        }
    }
}
