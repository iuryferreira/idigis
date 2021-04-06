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

        private string FullName { get; }
        private DateTime? BirthDate { get; }
        private DateTime? BaptismDate { get; }
        private Contact Contact { get; }

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
