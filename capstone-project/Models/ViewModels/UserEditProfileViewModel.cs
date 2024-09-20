using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.ViewModels
{
    public class UserEditProfileViewModel
    {
        // Immutable fields (cannot be edited)
        public string Username { get; set; }
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }

        // Editable fields
        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Il paese è obbligatorio.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "La città è obbligatoria.")]
        public string City { get; set; }

        [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Il codice postale è obbligatorio.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Il numero di telefono è obbligatorio.")]
        [Phone(ErrorMessage = "Numero di telefono non valido.")]
        public string PhoneNumber { get; set; }


        public byte[]? ProfileImg { get; set; }
        public IFormFile? NewProfileImg { get; set; }  // For uploading a new profile image
        public string? SelectedPredefinedImage { get; set; } // Selected pre-set image
    }
}
