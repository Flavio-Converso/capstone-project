namespace capstone_project.Models.DTOs
{
    public class WishlistItemDTO
    {
        public int WishlistItemId { get; set; }
        public int GameId { get; set; }
        public required string GameName { get; set; }
        public byte[]? CoverImage { get; set; }
    }
}
