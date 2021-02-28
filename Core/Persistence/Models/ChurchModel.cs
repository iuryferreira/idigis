using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Persistence.Models
{
    public class ChurchModel : Model
    {
        [Required] public string Name { get; init; }

        [Required] public string Email { get; init; }

        [Required] public string Password { get; init; }
    }
}
