using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class Wishlist
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistId { get; set; }

        //EF REFERENCES
        [Required]
        public required User User { get; set; }
        [Required]
        public List<Game> Games { get; set; } = [];

    }
}
