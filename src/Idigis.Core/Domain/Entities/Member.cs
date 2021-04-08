using System;
using FluentValidation;
using Idigis.Core.Domain.Contracts;
using Idigis.Core.Domain.Helpers;
using Idigis.Core.Domain.ValueObjects;

namespace Idigis.Core.Domain.Entities
{
    internal class Member : Entity
    {
        internal Member (string fullName, DateTime? birthDate = null, DateTime? baptismDate = null,
            Contact contact = null)
        {
            FullName = fullName;
            BirthDate = birthDate;
            BaptismDate = baptismDate;
            Contact = contact;
            Validate(this, new MemberValidator());
        }

        internal string FullName { get; }
        internal DateTime? BirthDate { get; }
        internal DateTime? BaptismDate { get; }
        internal Contact Contact { get; }

        private class MemberValidator : AbstractValidator<Member>
        {
            public MemberValidator ()
            {
                Include(new EntityValidator());
                RuleFor(member => member.Contact).SetValidator(new ContactValidator());
                RuleFor(member => member.FullName)
                    .NotEmpty().WithMessage(Messages.NotEmpty);
                RuleFor(member => member.BirthDate)
                    .LessThan(DateTime.Now).WithMessage(Messages.Date);
                RuleFor(member => member.BaptismDate)
                    .LessThan(DateTime.Now).WithMessage(Messages.Date);
            }
        }
    }
}
