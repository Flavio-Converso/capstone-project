namespace capstone_project.Models.DTOs.Wishlist
{
    public class WishlistItemDTO
    {
        public int WishlistItemId { get; set; }
        public int GameId { get; set; }
        public required string GameName { get; set; }
        public string GamePlatform { get; set; }
        public decimal Price { get; set; }
        public byte[]? CoverImage { get; set; }
    }
}
