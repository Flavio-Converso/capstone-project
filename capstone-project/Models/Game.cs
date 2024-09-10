using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Game
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required, MaxLength(1000)]
        public required string Description { get; set; }

        [Required, MaxLength(50)]
        public required string Platform { get; set; }

        [Required, MaxLength(100)]
        public required string Publisher { get; set; }

        [Required, Range(0.0, double.MaxValue)]
        public required decimal Price { get; set; }

        [Required]
        public required DateTime ReleaseDate { get; set; }

        [Required, Range(0, int.MaxValue)]
        public required int QuantityAvail { get; set; }

        // EF REFERENCES
        public List<GameImage> GameImages { get; set; } = [];
        public int PegiId { get; set; }
        [ForeignKey("PegiId")]
        public required Pegi Pegi { get; set; }

        public List<Restriction> Restrictions { get; set; } = [];

        public List<Category> Categories { get; set; } = [];

        public List<Review> Reviews { get; set; } = [];
    }
}
