using System;
using FluentValidation;
using FluentValidation.Results;

namespace Idigis.Core.Domain.Contracts
{
    internal abstract class Entity
    {
        protected Entity ()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; protected init; }
        public bool Valid { get; private set; }
        public bool Invalid => !Valid;
        public ValidationResult ValidationResult { get; private set; }

        protected bool Validate<T> (T entity, AbstractValidator<T> validator)
        {
            ValidationResult = validator.Validate(entity);
            return Valid = ValidationResult.IsValid;
        }
    }

    internal class EntityValidator : AbstractValidator<Entity>
    {
        public EntityValidator ()
        {
            RuleFor(entity => entity.Id).NotNull().NotEmpty().MinimumLength(36);
        }
    }
}
