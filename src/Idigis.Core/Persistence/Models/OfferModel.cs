using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;

namespace Idigis.Core.Persistence.Models
{
    [Table("offers")]
    internal class OfferModel : Model
    {
        [Required] public decimal Value { get; set; }
        [Required] public string ChurchId { get; set; }
        [Required] public ChurchModel Church { get; set; }

        public static implicit operator OfferModel (Offer entity)
        {
            if (entity is null) { return null; }

            return new() { Id = entity.Id, Value = entity.Value };
        }

        public static implicit operator Offer (OfferModel model)
        {
            if (model is null) { return null; }

            return new(model.Id, model.Value);
        }
    }
}
