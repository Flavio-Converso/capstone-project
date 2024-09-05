using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs.Wishlist;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly DataContext _ctx;

        public WishlistService(DataContext context)
        {
            _ctx = context;
        }

        public async Task<WishlistDTO> AddGameToWishlistAsync(int userId, int gameId)
        {
            var wishlist = await _ctx.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Game)
                .FirstOrDefaultAsync(w => w.UserId == userId);
            var user = await _ctx.Users.FindAsync(userId);
            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId,
                    User = user!
                };
                _ctx.Wishlists.Add(wishlist);
                await _ctx.SaveChangesAsync();
            }

            var game = await _ctx.Games.FindAsync(gameId);
            if (!wishlist.WishlistItems.Any(wi => wi.GameId == gameId))
            {
                var wishlistItem = new WishlistItem
                {
                    Wishlist = wishlist,
                    WishlistId = wishlist.WishlistId,
                    Game = game!,
                    GameId = gameId
                };
                _ctx.WishlistItems.Add(wishlistItem);
                await _ctx.SaveChangesAsync();
            }

            return new WishlistDTO
            {
                WishlistId = wishlist.WishlistId,
                UserId = wishlist.UserId,
                WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
                {
                    WishlistItemId = wi.WishlistItemId,
                    GameId = wi.GameId,
                    GameName = wi.Game.Name
                }).ToList()
            };
        }

        public async Task<bool> RemoveGameFromWishlistAsync(int userId, int gameId)
        {
            var wishlist = await _ctx.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            var item = wishlist!.WishlistItems.FirstOrDefault(wi => wi.GameId == gameId);
            _ctx.WishlistItems.Remove(item!);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WishlistItemDTO>> GetWishlistItemsAsync(int userId)
        {
            var wishlist = await _ctx.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Game)
                .ThenInclude(g => g.GameImages)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null) return new List<WishlistItemDTO>();

            return wishlist.WishlistItems.Select(wi => new WishlistItemDTO
            {
                WishlistItemId = wi.WishlistItemId,
                GameId = wi.GameId,
                GameName = wi.Game.Name,
                CoverImage = wi.Game.GameImages.FirstOrDefault(img => img.ImgType == ImageType.Cover)?.Img
            }).ToList();
        }

    }
}

