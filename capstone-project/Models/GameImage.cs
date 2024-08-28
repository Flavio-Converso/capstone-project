using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class GameImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameImageId { get; set; }
        [Required]
        public required byte[] Img { get; set; }
        [Required, MaxLength(50)]
        public required string ImgType { get; set; }

        //EF REFERENCES
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public required Game Game { get; set; }
    }
}
