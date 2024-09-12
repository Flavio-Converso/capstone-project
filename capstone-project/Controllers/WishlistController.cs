﻿using capstone_project.Helpers;
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

            switch (source)
            {
                case "List":
                    return RedirectToAction("List", "Game");
                case "Details":
                    return RedirectToAction("Details", "Game", new { id = gameId });
                default:
                    return RedirectToAction("List", "Game");
            }
        }

        // POST: /Game/RemoveFromWishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishlist(int gameId, string source)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _wishlistSvc.RemoveGameFromWishlistAsync(userId, gameId);

            switch (source)
            {
                case "List":
                    return RedirectToAction("List", "Game");
                case "Wishlist":
                    return RedirectToAction("Wishlist");
                case "Details":
                    return RedirectToAction("Details", "Game", new { id = gameId });
                default:
                    return RedirectToAction("List", "Game");
            }
        }
    }
}
