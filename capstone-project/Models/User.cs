using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]

    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public required string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public required string PasswordHash { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public required string Email { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        [Required, MaxLength(50)]
        public required string Surname { get; set; }

        [Required, DataType(DataType.Date)]
        public required DateTime BirthDate { get; set; }

        [Required, MaxLength(50)]
        public required string Country { get; set; }

        [Required, MaxLength(50)]
        public required string City { get; set; }

        [Required, MaxLength(100)]
        public required string Address { get; set; }

        [Required, StringLength(10)]
        public required string ZipCode { get; set; }

        [Required, Phone, MaxLength(15)]
        public required string PhoneNumber { get; set; }

        [Required]
        public required Gender Gender { get; set; }

        public byte[]? ProfileImg { get; set; }

        // EF REFERENCES 
        public List<Role> Roles { get; set; } = [];
        public List<Category> Categories { get; set; } = [];
    }
}
