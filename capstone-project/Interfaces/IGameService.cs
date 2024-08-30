using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IGameService
    {
        // Create
        Task<GameDTO> CreateGameAsync(GameDTO gameDto);

        // Read
        Task<Game> GetGameByIdAsync(int gameId);
        Task<IEnumerable<Game>> GetAllGamesAsync();

        // Update
        Task<GameDTO> UpdateGameAsync(int gameId, GameDTO updatedGameDto);

        // Delete
        Task<bool> DeleteGameAsync(int gameId);
    }
}
