using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs.Cart;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace capstone_project.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _ctx;
        private readonly IGameService _gameSvc;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IUserHelper _userHelper;

        public CartService(DataContext context, IGameService gameService, IConfiguration config, IEmailService emailService, IUserHelper userHelper)
        {
            _ctx = context;
            _gameSvc = gameService;
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            _emailService = emailService;
            _userHelper = userHelper;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(int userId)
        {
            var cart = await _ctx.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .ThenInclude(g => g.GameImages)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return new CartDTO { UserId = userId };

            return new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(ci => new CartItemDTO
                {
                    CartItemId = ci.CartItemId,
                    GameId = ci.GameId,
                    GameName = ci.Game.Name,
                    GamePlatform = ci.Game.Platform,
                    Quantity = ci.Quantity,
                    QuantityAvail = ci.Game.QuantityAvail,
                    Price = ci.Game.Price,
                    CoverImage = ci.Game.GameImages.FirstOrDefault(img => img.ImgType == ImageType.Cover)?.Img
                }).ToList()
            };
        }

        public async Task<CartDTO> AddGameToCartAsync(int userId, int gameId, int quantity)
        {
            var cart = await _ctx.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                var user = await _userHelper.GetUserIdAsync(userId);
                cart = new Cart
                {
                    UserId = userId,
                    User = user!
                };
                _ctx.Carts.Add(cart);
                await _ctx.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.GameId == gameId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var game = await _ctx.Games.FindAsync(gameId);
                cartItem = new CartItem
                {
                    Cart = cart,
                    GameId = gameId,
                    Game = game!,
                    Quantity = quantity
                };
                _ctx.CartItems.Add(cartItem);
            }

            await _ctx.SaveChangesAsync();

            return await GetCartByUserIdAsync(userId);
        }

        public async Task<bool> RemoveGameFromCartAsync(int userId, int gameId)
        {
            var cart = await _ctx.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.GameId == gameId);
            if (cartItem != null)
            {
                _ctx.CartItems.Remove(cartItem);
                await _ctx.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCartItemQuantityAsync(int userId, int gameId, int newQuantity)
        {
            var cart = await _ctx.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.GameId == gameId);
            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity;
                await _ctx.SaveChangesAsync();
                return true;
            }
            return false;
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

        public async Task CompleteCheckoutAsync(int userId)
        {
            var cart = await _ctx.Carts
                                 .Include(c => c.CartItems)
                                 .ThenInclude(ci => ci.Game)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new ArgumentException("The cart is empty or invalid.");
            }

            // Calculate the total amount
            var totalAmount = cart.CartItems.Sum(item => item.Game.Price * item.Quantity);

            // Get the username
            var user = await _userHelper.GetUserIdAsync(userId);
            var username = user!.Username;

            // Generate order number
            int lastOrderNumber = await _ctx.OrderSummaries
                                            .OrderByDescending(o => o.OrderNumber)
                                            .Select(o => o.OrderNumber)
                                            .FirstOrDefaultAsync();
            int orderNumber = lastOrderNumber > 100 ? lastOrderNumber + 1 : 101;

            // Concatenate game names and quantities for summary
            var gamesOrdered = string.Join(", ", cart.CartItems.Select(item =>
                $"{item.Game.Name} (x{item.Quantity})"));

            // Create the order summary
            var orderSummary = new OrderSummary
            {
                UserId = userId,
                Username = username,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                GamesOrdered = gamesOrdered,
                OrderNumber = orderNumber,
            };

            _ctx.OrderSummaries.Add(orderSummary);

            // Prepare email body with game details and keys
            string emailBody =
              $"<h3 style='color: #333;'>Dettagli del tuo ordine (ID ordine: {orderNumber}):</h3> " +  // Include order number here
               "<ul style='list-style-type: none; padding-left: 0;'>";

            // Process each item in the cart
            foreach (var cartItem in cart.CartItems)
            {
                // Generate keys for each game purchased
                var generatedKeys = await _gameSvc.GenerateGameKeysAsync(cartItem.GameId, userId, cartItem.Quantity);

                // Add game name and keys to email body
                emailBody += $@"
                        <li style='margin-bottom: 10px;'>
                            <strong>Gioco:</strong> {cartItem.Game.Name}
                            <br />
                            <strong>Chiave:</strong> {string.Join(", ", generatedKeys)}
                        </li>";

                // Decrease the quantity available for the game
                var game = cartItem.Game;
                game.QuantityAvail -= cartItem.Quantity;

                if (game.QuantityAvail < 0)
                {
                    game.QuantityAvail = 0;  // Ensure quantity does not go below 0
                }
            }

            emailBody += "</ul>" +
                "<p style='font-size: 14px; color: #555;'>Grazie per il tuo acquisto!</p>";

            // Clear the cart
            _ctx.CartItems.RemoveRange(cart.CartItems);

            // Save all changes to the database
            await _ctx.SaveChangesAsync();

            // Send email with keys (assuming you have an email service)
            await _emailService.SendEmailAsync(user.Email, "Le tue chiavi di gioco", emailBody);
        }
        public async Task<int> GetCartItemCountAsync(int userId)
        {
            var cart = await _ctx.Carts
                                 .Include(c => c.CartItems)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }

        public async Task<List<Game>> GetRelatedGamesFromCartAsync(int userId)
        {
            // Get the user's cart including games and their categories
            var cart = await _ctx.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .ThenInclude(g => g.Categories)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .ThenInclude(g => g.GameImages) // Ensure GameImages are included
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return new List<Game>(); // Return an empty list if the cart is empty
            }

            // Get all category IDs and platform names from games in the cart
            var gameCategories = cart.CartItems.SelectMany(ci => ci.Game.Categories.Select(c => c.CategoryId)).Distinct().ToList();
            var platforms = cart.CartItems.Select(ci => ci.Game.Platform).Distinct().ToList();

            // Query for related games based on category, platform, and availability
            var relatedGames = await _ctx.Games
                .Include(g => g.Categories)
                .Include(g => g.GameImages) // Include GameImages for each related game
                .Where(g => g.Categories.Any(c => gameCategories.Contains(c.CategoryId)) &&
                            platforms.Contains(g.Platform) &&
                            g.QuantityAvail > 0 && // Ensure the game has available quantity
                            !cart.CartItems.Select(ci => ci.GameId).Contains(g.GameId)) // Exclude games already in the cart
                .GroupBy(g => g.Name) // Group games by name to ensure uniqueness
                .Select(g => g.First()) // Select the first game for each unique name
                .Take(6) // Limit to 6 related games
                .ToListAsync();

            return relatedGames;
        }



    }
}