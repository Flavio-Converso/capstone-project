using capstone_project.Models;
using capstone_project.Models.DTOs.Game;

namespace capstone_project.Interfaces
{
    public interface IGameService
    {
        Task<GameDTO> CreateGameAsync(GameDTO gameDto);

        Task<Game> GetGameByIdAsync(int gameId);
        Task<IEnumerable<Game>> GetAllGamesAsync();

        Task<bool> UpdateGameAsync(GameDTO gameDto);

        Task<bool> DeleteGameAsync(int gameId);

        Task<List<string>> GenerateGameKeysAsync(int gameId, int userId, int quantity);
        Task<IEnumerable<Game>> SearchGamesAsync(string query);
    }
}
