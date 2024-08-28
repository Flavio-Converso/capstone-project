using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class Cart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        // EF REFERENCES
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        public List<CartItem> CartItems { get; set; } = [];
    }
}
