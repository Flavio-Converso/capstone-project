﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class CartItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public required int Quantity { get; set; }

        // EF REFERENCES
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public required Cart Cart { get; set; }

        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public required Game Game { get; set; }
    }
}
