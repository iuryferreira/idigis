using FluentValidation;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;

namespace Idigis.Core.Domain.Aggregates
{
    internal class Offer : Entity
    {
        public Offer (decimal value)
        {
            Value = value;
            Validate(this, new OfferValidator());
        }

        private decimal Value { get; }

        private class OfferValidator : AbstractValidator<Offer>
        {
            public OfferValidator ()
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
