using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class GameService : IGameService
    {
        private readonly DataContext _ctx;

        public GameService(DataContext context)
        {
            _ctx = context;
        }


        public async Task<GameDTO> CreateGameAsync(GameDTO gameDto)
        {
            var pegi = await _ctx.Pegis.FindAsync(gameDto.PegiId);

            var restrictions = await _ctx.Restrictions
                                      .Where(r => gameDto.RestrictionIds.Contains(r.RestrictionId))
                                      .ToListAsync();
            var categories = await _ctx.Categories
                                    .Where(c => gameDto.CategoryIds.Contains(c.CategoryId))
                                    .ToListAsync();

            var game = new Game
            {
                Name = gameDto.Name,
                Description = gameDto.Description,
                Platform = gameDto.Platform,
                Publisher = gameDto.Publisher,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                QuantityAvail = gameDto.QuantityAvail,
                Pegi = pegi!,
                Restrictions = restrictions,
                Categories = categories
            };

            // Associa l'oggetto Game a ciascuna GameImage
            game.GameImages = gameDto.GameImages.Select(imgDto => new GameImage
            {
                Img = imgDto.Img,
                ImgType = imgDto.ImgType,
                Game = game // Associazione con l'oggetto Game corrente
            }).ToList();

            _ctx.Games.Add(game);
            await _ctx.SaveChangesAsync();

            gameDto.GameId = game.GameId;
            return gameDto;
        }


        // Read
        public async Task<Game> GetGameByIdAsync(int gameId)
        {
            var game = await _ctx.Games
                .Include(g => g.Pegi)
                .Include(g => g.Restrictions)
                .Include(g => g.Categories)
                .FirstOrDefaultAsync(g => g.GameId == gameId);
            return game!;
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _ctx.Games
                .Include(g => g.Pegi)
                .Include(g => g.Restrictions)
                .Include(g => g.Categories)
                .Include(g => g.GameImages)
                .ToListAsync();
        }

        // Update
        public async Task<GameDTO> UpdateGameAsync(int gameId, GameDTO updatedGameDto)
        {
            var existingGame = await _ctx.Games
                .Include(g => g.Restrictions)
                .Include(g => g.Categories)
                .Include(g => g.GameImages)
                .FirstOrDefaultAsync(g => g.GameId == gameId);

            if (existingGame == null) return null;

            var pegi = await _ctx.Pegis.FindAsync(updatedGameDto.PegiId);

            existingGame.Name = updatedGameDto.Name;
            existingGame.Description = updatedGameDto.Description;
            existingGame.Platform = updatedGameDto.Platform;
            existingGame.Publisher = updatedGameDto.Publisher;
            existingGame.Price = updatedGameDto.Price;
            existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
            existingGame.QuantityAvail = updatedGameDto.QuantityAvail;
            existingGame.Pegi = pegi!;
            existingGame.Restrictions = await _ctx.Restrictions
                                                .Where(r => updatedGameDto.RestrictionIds.Contains(r.RestrictionId))
                                                .ToListAsync();
            existingGame.Categories = await _ctx.Categories
                                                .Where(c => updatedGameDto.CategoryIds.Contains(c.CategoryId))
                                                .ToListAsync();

            // Aggiorna le immagini del gioco
            existingGame.GameImages.Clear();
            existingGame.GameImages.AddRange(updatedGameDto.GameImages.Select(imgDto => new GameImage
            {
                Img = imgDto.Img,
                ImgType = imgDto.ImgType,
                Game = existingGame // Necessario per mantenere la relazione corretta
            }));

            await _ctx.SaveChangesAsync();
            return updatedGameDto;
        }

        // Delete
        public async Task<bool> DeleteGameAsync(int gameId)
        {
            var game = await _ctx.Games.FindAsync(gameId);

            _ctx.Games.Remove(game!);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
