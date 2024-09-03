using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.DTOs.Auth
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Il nome utente è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il nome utente non può superare i 50 caratteri.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria.")]
        [DataType(DataType.Password, ErrorMessage = "Il formato della password non è valido.")]
        public required string Password { get; set; }
    }
}
