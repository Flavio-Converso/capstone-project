using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class ReviewLike
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewLikeId { get; set; }

        public int ReviewId { get; set; }
        [ForeignKey("ReviewId")]
        public required Review Review { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
