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

        internal Tithe (string id, decimal value, DateTime date)
        {
            Id = id;
            Value = value;
            Date = date;
            Validate(this, new TitheValidator());
        }

        internal Tithe (string id, decimal value, DateTime date, string memberId, string memberName)
        {
            Id = id;
            Value = value;
            Date = date;
            MemberId = memberId;
            MemberName = memberName;
        }

        internal DateTime Date { get; }
        internal decimal Value { get; }
        internal string MemberId { get; }
        internal string MemberName { get; }

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
