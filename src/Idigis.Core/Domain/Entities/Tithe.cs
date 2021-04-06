using System;
using FluentValidation;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;

namespace Idigis.Core.Domain.Entities
{
    internal class Tithe : Entity
    {
        internal Tithe (decimal value, DateTime date, string memberId)
        {
            Value = value;
            Date = date;
            MemberId = memberId;
            Validate(this, new TitheValidator());
        }

        private string MemberId { get; }
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
                RuleFor(tithe => tithe.MemberId).NotEmpty()
                    .WithMessage(Messages.NotEmpty);
                RuleFor(tithe => tithe.Date)
                    .NotEmpty().WithMessage(Messages.NotEmpty);
            }
        }
    }
}
