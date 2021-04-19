using System.ComponentModel.DataAnnotations;

namespace Idigis.Shared.Dtos.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email precisa ser válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MinLength(8, ErrorMessage = "A senha precisa ser maior que 8 caracteres.")]
        public string Password { get; set; }
        public string Hash { get; set; }
        public string ChurchId { get; set; }
        public string Name { get; set; }
    }
}
