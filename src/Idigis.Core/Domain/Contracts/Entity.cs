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

        public string Id { get; }
        private bool Valid { get; set; }
        public bool Invalid => !Valid;
        internal ValidationResult ValidationResult { get; private set; }

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
