using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.DTOs.Auth
{
    public class UserRegisterDTO
    {
        // public int UserId { get; set; }

        [Required(ErrorMessage = "Il nome utente è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il nome utente non può superare i 50 caratteri.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria.")]
        [DataType(DataType.Password, ErrorMessage = "Il formato della password non è valido.")]
        public required string PasswordHash { get; set; }

        [Required(ErrorMessage = "La conferma della password è obbligatoria.")]
        [Compare("PasswordHash", ErrorMessage = "Le password non coincidono.")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria.")]
        [EmailAddress(ErrorMessage = "Formato email non valido.")]
        [MaxLength(100, ErrorMessage = "L'email non può superare i 100 caratteri.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri.")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "La data di nascita è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Il formato della data di nascita non è valido.")]
        public DateTime? BirthDate { get; set; }


        [Required(ErrorMessage = "Il paese è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il paese non può superare i 50 caratteri.")]
        public required string Country { get; set; }

        [Required(ErrorMessage = "La città è obbligatoria.")]
        [MaxLength(50, ErrorMessage = "La città non può superare i 50 caratteri.")]
        public required string City { get; set; }

        [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "L'indirizzo non può superare i 100 caratteri.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Il CAP è obbligatorio.")]
        [StringLength(10, ErrorMessage = "Il CAP non può superare i 10 caratteri.")]
        public required string ZipCode { get; set; }

        [Required(ErrorMessage = "Il numero di telefono è obbligatorio.")]
        [Phone(ErrorMessage = "Il formato del numero di telefono non è valido.")]
        [MaxLength(15, ErrorMessage = "Il numero di telefono non può superare i 15 caratteri.")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Il genere è obbligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleziona un genere valido.")]
        public Gender Gender { get; set; }

        public string? SelectedPredefinedImage { get; set; }

        public IFormFile? Img { get; set; }

        // EF REFERENCES 
        // public List<Role> Roles { get; set; } = [];
        public List<int> SelectedCategories { get; set; } = [];
    }
}
