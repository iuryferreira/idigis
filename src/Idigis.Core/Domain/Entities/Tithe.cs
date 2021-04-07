using System;
using FluentValidation;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;

namespace Idigis.Core.Domain.Entities
{
    internal class Tithe : Entity
    {
        internal Tithe (decimal value, DateTime date)
        {
            Value = value;
            Date = date;
            Validate(this, new TitheValidator());
        }

        private DateTime Date { get; }
        private decimal Value { get; }

        private class TitheValidator : AbstractValidator<Tithe>
        {
            internal TitheValidator ()
            {
                Include(new EntityValidator());
                RuleFor(tithe => tithe.Value).NotNull()
                    .WithMessage(Messages.NotEmpty)
                    .GreaterThan(0)
                    .WithMessage(Messages.Minimum(0, false));
                RuleFor(tithe => tithe.Date)
                    .NotEmpty().WithMessage(Messages.NotEmpty);
            }
        }
    }
}
