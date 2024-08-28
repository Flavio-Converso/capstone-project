using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class WishlistItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistItemId { get; set; }

        // EF REFERENCES
        public int WishlistId { get; set; }

        [ForeignKey("WishlistId")]
        public required Wishlist Wishlist { get; set; }

        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public required Game Game { get; set; }
    }
}