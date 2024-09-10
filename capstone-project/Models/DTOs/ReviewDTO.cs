using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models.DTOs
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }

        [Required, MaxLength(2000)]
        public required string Content { get; set; }

        [Required]
        public required DateTime Date { get; set; } = DateTime.Now;

        [Required, Range(0.0, 5.0)]
        public required decimal Rating { get; set; }

        [Required, Range(0, int.MaxValue)]
        public required int Likes { get; set; }

        // EF REFERENCES
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public required User User { get; set; }

        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public required Models.Game Game { get; set; }
        public List<ReviewLike> ReviewLikes { get; set; } = [];

    }
}
