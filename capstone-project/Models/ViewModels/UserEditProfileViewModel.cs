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
        [Required(ErrorMessage = "The surname is required.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "The country is required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "The city is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "The address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The ZIP code is required.")]
        public string ZipCode { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        public byte[]? ProfileImg { get; set; }
        public IFormFile? NewProfileImg { get; set; }  // For uploading a new profile image
        public string? SelectedPredefinedImage { get; set; } // Selected pre-set image
    }
}
