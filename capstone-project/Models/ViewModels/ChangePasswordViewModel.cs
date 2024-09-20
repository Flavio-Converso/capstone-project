using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "La password attuale è obbligatoria.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "La nuova password è obbligatoria.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Conferma la nuova password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "La nuova password e la conferma non corrispondono.")]
        public string ConfirmNewPassword { get; set; }
    }
}
