using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Pegi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PegiId { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [Required]
        public required byte[] Img { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
