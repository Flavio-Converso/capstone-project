﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }

        [Required, MaxLength(2000)]
        public required string Content { get; set; }

        [Required]
        public required DateTime Date { get; set; } = DateTime.Now;

        [Required, Range(0, 5)]
        public required decimal Rating { get; set; }

        // EF REFERENCES


        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public List<ReviewLike> ReviewLikes { get; set; } = [];

    }
}
