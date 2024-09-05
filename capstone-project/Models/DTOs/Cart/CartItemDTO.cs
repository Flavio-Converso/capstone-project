namespace capstone_project.Models.DTOs.Cart
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int Quantity { get; set; }
        public int QuantityAvail { get; set; }
        public decimal Price { get; set; }
        public byte[]? CoverImage { get; set; }
    }
}
