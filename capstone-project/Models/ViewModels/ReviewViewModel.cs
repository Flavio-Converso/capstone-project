using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.ViewModels
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "Per favore inserisci un titolo.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Per favore inserisci una recensione")]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Per favore inserisci un voto da 1 a 5")]
        [Range(1.0, 5.0, ErrorMessage = "La Valutazione deve essere compresa tra 1 e 5.")]
        public decimal Rating { get; set; } = 1;

        public int GameId { get; set; }
    }
}
