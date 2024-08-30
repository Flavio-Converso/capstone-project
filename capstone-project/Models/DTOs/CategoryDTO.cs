using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Devi inserire un nome:"), MaxLength(50, ErrorMessage = "Il nome non deve superare i 50 caratteri:")]
        public required string Name { get; set; }
        [MaxLength(250, ErrorMessage = "La descrizione non deve superare i 255 caratteri:")]
        public string? Description { get; set; }


        // EF REFERENCES
        public List<User> Users { get; set; } = [];

        public List<Game> Games { get; set; } = [];
    }
}
