using Core.Shared.Entities;
using Core.Shared.Notifications;
using FluentValidation;

namespace Core.Domain.Aggregates
{
    public class Offer : Entity
    {
        public decimal Value { get; }

        public Offer (decimal value)
        {
            Value = value;
            Validate(this, new OfferValidator());
        }
    }

    public class OfferValidator : AbstractValidator<Offer>
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
