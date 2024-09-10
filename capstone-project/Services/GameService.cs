using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs.Game;
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
            if (await _ctx.Games.AnyAsync(g => g.Name == gameDto.Name))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }

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

            game.GameImages = gameDto.GameImages.Select(imgDto => new GameImage
            {
                Img = imgDto.Img,
                ImgType = imgDto.ImgType,
                Game = game
            }).ToList();

            _ctx.Games.Add(game);
            await _ctx.SaveChangesAsync();

            gameDto.GameId = game.GameId;
            return gameDto;
        }


        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            var games = await _ctx.Games
             .Include(g => g.Pegi)
             .Include(g => g.Restrictions)
             .Include(g => g.Categories)
             .Include(g => g.GameImages)
             .ToListAsync();
            return games;
        }

        public async Task<Game> GetGameByIdAsync(int gameId)
        {
            var game = await _ctx.Games
                .Include(g => g.Pegi)
                .Include(g => g.Restrictions)
                .Include(g => g.Categories)
                .Include(g => g.GameImages)
                .Include(g => g.Reviews)
                .ThenInclude(r => r.User)
                .Include(g => g.Reviews)
                .ThenInclude(r => r.ReviewLikes)
                .FirstOrDefaultAsync(g => g.GameId == gameId);
            return game!;
        }

        public async Task<bool> UpdateGameAsync(GameDTO gameDto)
        {
            if (await _ctx.Games.AnyAsync(g => g.Name == gameDto.Name && g.GameId != gameDto.GameId))
            {
                throw new ArgumentException("Il nome del gioco specificato è già in uso.");
            }

            var game = await _ctx.Games
                .Include(g => g.GameImages)
                .Include(g => g.Restrictions)
                .Include(g => g.Categories)
                .FirstOrDefaultAsync(g => g.GameId == gameDto.GameId);

            game!.Name = gameDto.Name;
            game.Description = gameDto.Description;
            game.Platform = gameDto.Platform;
            game.Publisher = gameDto.Publisher;
            game.Price = gameDto.Price;
            game.ReleaseDate = gameDto.ReleaseDate;
            game.QuantityAvail = gameDto.QuantityAvail;

            game.Pegi = await _ctx.Pegis.FindAsync(gameDto.PegiId) ?? throw new ArgumentException("Invalid PEGI ID.");

            game.Restrictions = await _ctx.Restrictions
                .Where(r => gameDto.RestrictionIds.Contains(r.RestrictionId))
                .ToListAsync();

            game.Categories = await _ctx.Categories
                .Where(c => gameDto.CategoryIds.Contains(c.CategoryId))
                .ToListAsync();

            foreach (var imgDto in gameDto.GameImages)
            {
                var existingImage = game.GameImages.FirstOrDefault(img => img.ImgType == imgDto.ImgType);

                if (imgDto.Img != null && imgDto.Img.Length > 0)
                {
                    if (existingImage != null)
                    {
                        existingImage.Img = imgDto.Img;
                    }
                    else
                    {
                        game.GameImages.Add(new GameImage
                        {
                            Img = imgDto.Img,
                            ImgType = imgDto.ImgType,
                            Game = game
                        });
                    }
                }
            }

            await _ctx.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteGameAsync(int gameId)
        {
            var game = await _ctx.Games.FindAsync(gameId);

            _ctx.Games.Remove(game!);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
