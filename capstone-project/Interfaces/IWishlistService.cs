using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistDTO> GetWishlistByUserIdAsync(int userId);
        Task<WishlistDTO> AddGameToWishlistAsync(int userId, int gameId);
        Task<bool> RemoveGameFromWishlistAsync(int userId, int gameId);
        Task<IEnumerable<WishlistItemDTO>> GetWishlistItemsAsync(int userId);
    }
}
