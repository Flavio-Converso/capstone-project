namespace capstone_project.Models.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
        public decimal TotalPrice => CartItems.Sum(item => item.Quantity * item.Price);
    }
}
