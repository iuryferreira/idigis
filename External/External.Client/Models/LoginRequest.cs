using System.ComponentModel.DataAnnotations;

namespace External.Client.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MinLength(4, ErrorMessage = "O nome do usuário precisa ser maior que 4 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MinLength(4, ErrorMessage = "A senha precisa ser maior que 4 caracteres.")]
        public string Password { get; set; }
    }
}
