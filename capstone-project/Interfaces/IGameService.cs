using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IGameService
    {
        Task<GameDTO> CreateGameAsync(GameDTO gameDto);

        Task<Game> GetGameByIdAsync(int gameId);
        Task<IEnumerable<Game>> GetAllGamesAsync();

        Task<bool> UpdateGameAsync(GameDTO gameDto);

        Task<bool> DeleteGameAsync(int gameId);
    }
}
