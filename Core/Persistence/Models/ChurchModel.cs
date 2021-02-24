using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Persistence.Models
{
    public class ChurchModel : Model
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
