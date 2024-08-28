using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }


        // EF REFERENCES
        public List<User> Users { get; set; } = [];

        public List<Game> Games { get; set; } = [];
    }
}
