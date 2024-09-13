using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models.DTOs
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Please enter a title for your review"), MaxLength(100, ErrorMessage = "The title cannot exceed 100 characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Please provide content for your review"), MaxLength(2000, ErrorMessage = "The review content cannot exceed 2000 characters")]
        public required string Content { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please give a rating between 0 and 5"), Range(0.0, 5.0, ErrorMessage = "Rating must be between 0 and 5")]
        public required decimal Rating { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Likes { get; set; }

        // EF REFERENCES
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public Models.Game Game { get; set; }
        public List<ReviewLike> ReviewLikes { get; set; } = [];

    }
}
