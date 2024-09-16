using capstone_project.Helpers;
using capstone_project.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistSvc;
        private readonly IUserHelper _userHelper;
        public WishlistController(IWishlistService wishlistService, IUserHelper userHelper)
        {
            _wishlistSvc = wishlistService;
            _userHelper = userHelper;
        }
        // GET: /Game/Wishlist
        public async Task<IActionResult> Wishlist()
        {
            var userId = _userHelper.GetUserIdClaim();
            var wishlistItems = await _wishlistSvc.GetWishlistItemsAsync(userId);
            return View(wishlistItems);
        }

        // POST: /Game/AddToWishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishlist(int gameId, string source)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _wishlistSvc.AddGameToWishlistAsync(userId, gameId);

            // Return a JSON response to indicate success
            return Json(new { success = true, message = "Game added to wishlist." });
        }


        // POST: /Game/RemoveFromWishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishlist(int gameId, string source)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _wishlistSvc.RemoveGameFromWishlistAsync(userId, gameId);

            // Return a JSON response to indicate success
            return Json(new { success = true, message = "Game removed from wishlist." });
        }


        public async Task<IActionResult> GetWishlistItemCount()
        {
            var userId = _userHelper.GetUserIdClaim();

            if (userId == null)
            {
                return Json(new { success = false, count = 0, message = "User not authenticated" });
            }

            var wishlistItems = await _wishlistSvc.GetWishlistItemsAsync(userId);
            var itemCount = wishlistItems.Count();

            return Json(new { success = true, count = itemCount });
        }
    }
}
