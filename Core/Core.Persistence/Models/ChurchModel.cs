using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities;
using Core.Shared.Models;

namespace Core.Persistence.Models
{
    [Table("churches")]
    public class ChurchModel : Model
    {
        [Required] public string Name { get; set; }

        [Required] public string Email { get; set; }

        [Required] public string Password { get; set; }

        public List<OfferModel> Offers { get; set; }

        public static implicit operator ChurchModel (Church entity)
        {
            if (entity is null) { return null; }
            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Credentials.Email,
                Password = entity.Credentials.Password
            };
        }
        public static implicit operator Church (ChurchModel model)
        {
            if (model is null) { return null; }
            return new(model.Id, model.Name, new(model.Email, model.Password));
        }
    }
}
