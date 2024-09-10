using capstone_project.Interfaces;
using capstone_project.Models.DTOs.Cart;
using Stripe;
using Stripe.Checkout;

namespace capstone_project.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _config;

        public StripeService(IConfiguration config)
        {
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<string> CreateCheckoutSessionAsync(CartDTO cart, string successUrl)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.CartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.GameName
                        },
                        UnitAmount = (long)(item.Price * 100)
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = successUrl,
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }
    }
}