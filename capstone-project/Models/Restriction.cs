using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Restriction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RestrictionId { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [Required]
        public required byte[] Img { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        //EF REFERENCES
        public List<Game> Games { get; set; } = [];
    }
}
