
using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace capstone_project.Controllers
{
    public class GameController : Controller
    {
        private readonly DataContext _ctx;
        private readonly IGameService _gameSvc;
        private readonly IWishlistService _wishlistSvc;
        private readonly ICartService _cartSvc;
        private readonly IReviewService _reviewSvc;
        private readonly IImgValidateHelper _imgValidateHelper;
        private readonly IUserHelper _userHelper;

        public GameController(IGameService gameService, IWishlistService wishlistService, ICartService cartService,
            DataContext context, IImgValidateHelper imgValidateHelper, IUserHelper userHelper, IReviewService reviewSvc)
        {
            _gameSvc = gameService;
            _wishlistSvc = wishlistService;
            _cartSvc = cartService;
            _ctx = context;
            _imgValidateHelper = imgValidateHelper;
            _userHelper = userHelper;
            _reviewSvc = reviewSvc;
        }
        private void PopulateViewBags()
        {
            ViewBag.PegiOptions = new SelectList(_ctx.Pegis, "PegiId", "Name");
            ViewBag.RestrictionOptions = new SelectList(_ctx.Restrictions, "RestrictionId", "Name");
            ViewBag.CategoryOptions = new SelectList(_ctx.Categories, "CategoryId", "Name");
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
            foreach (var imageUpload in images)
            {
                if (!_imgValidateHelper.IsValidImage(imageUpload.ImageFile!, out string errorMessage))
                {
                    ModelState.AddModelError("GameImages", errorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateViewBags();
                return View(gameDto);
            }

            try
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
                                ImgType = Enum.Parse<ImageType>(imageUpload.ImgType!)
                            });
                        }
                    }
                }

                await _gameSvc.CreateGameAsync(gameDto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateViewBags();
                return View(gameDto);
            }
        }

        // GET: /Game
        public async Task<IActionResult> List(string category, string platform)
        {
            var games = await _gameSvc.GetAllGamesAsync();

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                games = games.Where(g => g.Categories.Any(c => c.Name == category)).ToList();
            }

            // Filter by platform if provided
            if (!string.IsNullOrEmpty(platform))
            {
                games = games.Where(g => g.Platform == platform).ToList();
            }

            var userId = _userHelper.GetUserIdClaim();
            var wishlistItems = await _wishlistSvc.GetWishlistItemsAsync(userId);
            var wishlistGameIds = wishlistItems.Select(w => w.GameId).ToList();

            var cart = await _cartSvc.GetCartByUserIdAsync(userId);
            var cartGameIds = cart.CartItems.Select(c => c.GameId).ToList();

            ViewBag.WishlistGameIds = wishlistGameIds;
            ViewBag.CartGameIds = cartGameIds;

            // Pass the platform and category to the view
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedPlatform = platform;

            return View(games);
        }





        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameSvc.GetGameByIdAsync(id);

            var userId = _userHelper.GetUserIdClaim();
            var wishlistItems = await _wishlistSvc.GetWishlistItemsAsync(userId);
            var isInWishlist = wishlistItems.Any(w => w.GameId == id);

            var cart = await _cartSvc.GetCartByUserIdAsync(userId);
            var cartGameIds = cart.CartItems.Select(c => c.GameId).ToList();

            ViewBag.IsInWishlist = isInWishlist;
            ViewBag.CartGameIds = cartGameIds;

            bool hasReviewed = await _reviewSvc.HasUserReviewedGameAsync(userId, id);
            ViewBag.HasReviewed = hasReviewed;

            // Get reviews the user has liked
            var likedReviewIds = _ctx.ReviewLikes
                .Where(rl => rl.UserId == userId)
                .Select(rl => rl.ReviewId)
                .ToList();
            ViewBag.LikedReviews = likedReviewIds;

            var likeCounts = new Dictionary<int, int>();
            foreach (var review in game.Reviews)
            {
                likeCounts[review.ReviewId] = await _reviewSvc.GetReviewLikeCountAsync(review.ReviewId);
            }
            ViewBag.LikeCounts = likeCounts;

            return View(game);
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
                CategoryIds = game.Categories.Select(c => c.CategoryId).ToList(),
                GameImages = game.GameImages.Select(img => new GameImageDTO
                {
                    Img = img.Img,
                    ImgType = img.ImgType
                }).ToList()
            };

            PopulateViewBags();
            return View(gameDto);
        }

        // POST: /Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GameDTO gameDto, List<ImageUpload> images)
        {
            // Validate image extensions
            foreach (var imageUpload in images)
            {
                if (imageUpload.ImageFile != null && imageUpload.ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(imageUpload.ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("GameImages", "Sono consentiti solo file JPG e PNG.");
                        continue;
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await imageUpload.ImageFile.CopyToAsync(memoryStream);
                        var imgType = Enum.Parse<ImageType>(imageUpload.ImgType!);

                        var existingImage = gameDto.GameImages.FirstOrDefault(img => img.ImgType == imgType);

                        if (existingImage != null)
                        {
                            existingImage.Img = memoryStream.ToArray();
                        }
                        else
                        {
                            gameDto.GameImages.Add(new GameImageDTO
                            {
                                Img = memoryStream.ToArray(),
                                ImgType = imgType
                            });
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateViewBags();
                return View(gameDto);
            }

            try
            {
                await _gameSvc.UpdateGameAsync(gameDto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateViewBags();
                return View(gameDto);
            }
        }

        // GET: /Game/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameSvc.GetGameByIdAsync(id);

            return View(game);
        }

        // POST: /Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gameSvc.DeleteGameAsync(id);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(new { success = false, message = "Query cannot be empty." });
            }

            // Call the search method in the service
            var games = await _gameSvc.SearchGamesAsync(name);

            // Ensure each property is being returned correctly, including handling for null values
            var gameResults = games.Select(game => new
            {
                GameId = game.GameId,
                GameName = game.Name ?? "No name available",
                Platform = game.Platform ?? "No platform available",
                Price = game.Price, // Default price to 0 if null
                CoverImage = game.GameImages.FirstOrDefault(img => img.ImgType == ImageType.Cover)?.Img != null
                                ? Convert.ToBase64String(game.GameImages.FirstOrDefault(img => img.ImgType == ImageType.Cover).Img)
                                : null
            });

            return Json(new { success = true, games = gameResults });
        }



    }
}