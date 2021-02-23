using System;
using FluentValidation;

namespace Shared.Entities
{
    public abstract class Entity
    {
        public string Id { get; protected set; }

        protected Entity ()
        {
            Id = Guid.NewGuid().ToString();
        }

    }

    public class EntityValidator : AbstractValidator<Entity>
    {
        public EntityValidator ()
        {
            RuleFor(entity => entity.Id).NotNull().NotEmpty().MinimumLength(36);
        }
    }
}
