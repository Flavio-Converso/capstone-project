using capstone_project.Models.DTOs.Cart;

namespace capstone_project.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(CartDTO cart, string successUrl);
    }
}
