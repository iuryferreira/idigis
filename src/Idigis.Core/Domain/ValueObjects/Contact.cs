using FluentValidation;
using Idigis.Core.Domain.Helpers;

namespace Idigis.Core.Domain.ValueObjects
{
    internal class Contact
    {
        public Contact (string phoneNumber, string houseNumber, string street, string district, string city)
        {
            PhoneNumber = phoneNumber;
            HouseNumber = houseNumber;
            Street = street;
            District = district;
            City = city;
        }

        internal string PhoneNumber { get; }
        internal string HouseNumber { get; }
        internal string Street { get; }
        internal string District { get; }
        internal string City { get; }
    }

    internal class ContactValidator : AbstractValidator<Contact>
    {
        internal ContactValidator ()
        {
            RuleFor(contact => contact.PhoneNumber)
                .MinimumLength(11).WithMessage(Messages.Minimum(11));
            RuleFor(contact => contact.HouseNumber)
                .NotEmpty().WithMessage(Messages.NotEmpty);
            RuleFor(contact => contact.Street)
                .NotEmpty().WithMessage(Messages.NotEmpty);
            RuleFor(contact => contact.District)
                .NotEmpty().WithMessage(Messages.NotEmpty);
            RuleFor(contact => contact.City)
                .NotEmpty().WithMessage(Messages.NotEmpty);
        }
    }
}
