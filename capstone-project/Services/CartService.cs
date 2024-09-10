using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs.Cart;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _ctx;

        public CartService(DataContext context)
        {
            _ctx = context;
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
                cart = new Cart
                {
                    UserId = userId,
                    User = await _ctx.Users.FindAsync(userId)
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
                cartItem = new CartItem
                {
                    Cart = cart,
                    GameId = gameId,
                    Game = await _ctx.Games.FindAsync(gameId),
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
    }
}
