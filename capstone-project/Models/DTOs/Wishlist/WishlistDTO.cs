namespace capstone_project.Models.DTOs.Wishlist
{
    public class WishlistDTO
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public List<WishlistItemDTO> WishlistItems { get; set; } = [];
    }
}