using FluentValidation;
using Shared.Entities;

namespace Domain.Entities
{
    public class Church : Entity
    {
        public string Name { get; private set; }

        public Church (string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class ChurchValidator : AbstractValidator<Church>
    {
        public ChurchValidator ()
        {
            Include(new EntityValidator());
            RuleFor(church => church.Name).NotEmpty().NotNull();
        }
    }
}
