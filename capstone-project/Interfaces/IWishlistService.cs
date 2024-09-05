using capstone_project.Models.DTOs.Wishlist;

namespace capstone_project.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistDTO> AddGameToWishlistAsync(int userId, int gameId);
        Task<bool> RemoveGameFromWishlistAsync(int userId, int gameId);
        Task<IEnumerable<WishlistItemDTO>> GetWishlistItemsAsync(int userId);
    }
}
