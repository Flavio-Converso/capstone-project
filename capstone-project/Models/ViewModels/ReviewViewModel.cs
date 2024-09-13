using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.ViewModels
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "The title is required.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "The content is required.")]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required(ErrorMessage = "The rating is required.")]
        [Range(0.0, 5.0, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal Rating { get; set; }

        public int GameId { get; set; }
    }
}
