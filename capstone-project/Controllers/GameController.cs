using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace capstone_project.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameSvc;
        private readonly DataContext _ctx;

        public GameController(IGameService gameService, DataContext context)
        {
            _gameSvc = gameService;
            _ctx = context;
        }
        private void PopulateViewBags()
        {
            ViewBag.PegiOptions = new SelectList(_ctx.Pegis, "PegiId", "Name");
            ViewBag.RestrictionOptions = new SelectList(_ctx.Restrictions, "RestrictionId", "Name");
            ViewBag.CategoryOptions = new SelectList(_ctx.Categories, "CategoryId", "Name");
        }
        // GET: /Game
        public async Task<IActionResult> List()
        {
            var games = await _gameSvc.GetAllGamesAsync();
            return View(games);
        }

        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameSvc.GetGameByIdAsync(id);

            return View(game); // Visualizza i dettagli del gioco
        }

        // GET: /Game/Create
        public IActionResult Create()
        {
            PopulateViewBags();
            return View();
        }

        // POST: /Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameDTO gameDto, List<ImageUpload> images)
        {
            if (ModelState.IsValid)
            {
                foreach (var imageUpload in images)
                {
                    if (imageUpload.ImageFile != null && imageUpload.ImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageUpload.ImageFile.CopyToAsync(memoryStream);
                            gameDto.GameImages.Add(new GameImageDTO
                            {
                                Img = memoryStream.ToArray(),
                                ImgType = Enum.Parse<ImageType>(imageUpload.ImgType)
                            });
                        }
                    }
                }

                await _gameSvc.CreateGameAsync(gameDto);
                return RedirectToAction("List");
            }
            PopulateViewBags();
            return View(gameDto);
        }




        // GET: /Game/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameSvc.GetGameByIdAsync(id);

            var gameDto = new GameDTO
            {
                GameId = game.GameId,
                Name = game.Name,
                Description = game.Description,
                Platform = game.Platform,
                Publisher = game.Publisher,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                QuantityAvail = game.QuantityAvail,
                PegiId = game.Pegi.PegiId,
                RestrictionIds = game.Restrictions.Select(r => r.RestrictionId).ToList(),
                CategoryIds = game.Categories.Select(c => c.CategoryId).ToList()
            };

            PopulateViewBags();
            return View(gameDto); // Passa il GameDTO alla vista
        }


        // POST: /Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameDTO gameDto)
        {

            if (ModelState.IsValid)
            {
                var updatedGame = await _gameSvc.UpdateGameAsync(id, gameDto);

                return RedirectToAction("List"); // Dopo l'aggiornamento, ritorna alla lista dei giochi
            }
            PopulateViewBags();
            return View(gameDto); // Se ci sono errori di validazione, ritorna al form di modifica
        }

        // GET: /Game/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameSvc.GetGameByIdAsync(id);

            return View(game); // Mostra la vista di conferma dell'eliminazione con i dettagli del gioco
        }

        // POST: /Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _gameSvc.DeleteGameAsync(id);

            return RedirectToAction("List"); // Dopo l'eliminazione, ritorna alla lista dei giochi
        }

    }
}
