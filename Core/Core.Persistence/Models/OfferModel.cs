using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Shared.Models;

namespace Core.Persistence.Models
{
    [Table("offers")]
    public class OfferModel : Model
    {
        [Required] public decimal Value { get; init; }
        [Required] public ChurchModel Church { get; init; }
    }
}
