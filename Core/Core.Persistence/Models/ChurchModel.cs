using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Shared.Models;

namespace Core.Persistence.Models
{
    public class ChurchModel : Model
    {
        [Required] public string Name { get; init; }

        [Required] public string Email { get; init; }

        [Required] public string Password { get; init; }

        public static implicit operator ChurchModel (Church entity)
        {
            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Credentials.Email,
                Password = entity.Credentials.Password
            };
        }
    }
}
