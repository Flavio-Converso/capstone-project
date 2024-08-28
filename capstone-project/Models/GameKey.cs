using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    [Index(nameof(KeyNum), IsUnique = true)]
    public class GameKey
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameKeyId { get; set; }

        [Required, MaxLength(100)]
        public required string KeyNum { get; set; }

        // EF REFERENCES
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public required Game Game { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
