using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartSvc;
        private readonly IUserHelper _userHelper;
        public CartController(ICartService cartService, IUserHelper userHelper)
        {
            _cartSvc = cartService;
            _userHelper = userHelper;
        }

        // GET: /Game/Cart
        public async Task<IActionResult> Cart()
        {
            var userId = _userHelper.GetUserIdClaim();
            var cart = await _cartSvc.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                ViewBag.RelatedGames = new List<Game>(); // No related games if cart is empty
                return View(cart); // Pass empty cart
            }

            // Get the list of game names in the cart
            var cartGameNames = cart.CartItems.Select(ci => ci.GameName).Distinct().ToList();

            // Fetch related games based on the user's cart contents, excluding games with the same name as those in the cart
            var relatedGames = await _cartSvc.GetRelatedGamesFromCartAsync(userId);

            // Exclude games that are already in the cart by name
            relatedGames = relatedGames.Where(g => !cartGameNames.Contains(g.Name)).ToList();

            ViewBag.RelatedGames = relatedGames; // Pass the filtered related games to the view

            return View(cart); // Pass the cart model to the view
        }




        // POST: /Game/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int gameId, string source)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _cartSvc.AddGameToCartAsync(userId, gameId, 1);

            // Return a JSON response instead of a redirect
            return Json(new { success = true, message = "Game added to cart." });
        }

        // POST: /Game/RemoveFromCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int gameId, string source)
        {
            var userId = _userHelper.GetUserIdClaim();
            var result = await _cartSvc.RemoveGameFromCartAsync(userId, gameId);

            if (result)
            {
                // Get the updated cart details to return updated totals
                var cart = await _cartSvc.GetCartByUserIdAsync(userId);
                var cartTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);

                return Json(new
                {
                    success = true,
                    cartTotal = cartTotal // Return updated cart total
                });
            }

            return Json(new { success = false, message = "Failed to remove the game from the cart." });
        }




        // POST: /Game/UpdateCartItemQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItemQuantity(int gameId, int quantity)
        {
            var userId = _userHelper.GetUserIdClaim();

            if (quantity < 1)
            {
                quantity = 1;
            }

            await _cartSvc.UpdateCartItemQuantityAsync(userId, gameId, quantity);

            // Get the updated cart details to return updated totals
            var cart = await _cartSvc.GetCartByUserIdAsync(userId);
            var item = cart.CartItems.FirstOrDefault(ci => ci.GameId == gameId);
            var itemTotal = item.Quantity * item.Price;
            var cartTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);

            // Return JSON response with updated item and cart totals
            return Json(new
            {
                success = true,
                itemTotal = itemTotal,
                cartTotal = cartTotal,
                gameName = item.GameName
            });
        }


        public async Task<IActionResult> Checkout()
        {
            var userId = _userHelper.GetUserIdClaim();
            var cart = await _cartSvc.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Cart");
            }

            var successUrl = Url.Action("CheckoutSuccess", "Cart", null, Request.Scheme);

            var checkoutUrl = await _cartSvc.CreateCheckoutSessionAsync(cart, successUrl!);

            return Redirect(checkoutUrl);
        }

        public async Task<IActionResult> CheckoutSuccess()
        {
            var userId = _userHelper.GetUserIdClaim();

            await _cartSvc.CompleteCheckoutAsync(userId);

            return View();
        }

        public async Task<IActionResult> GetCartItemCount()
        {
            var userId = _userHelper.GetUserIdClaim();

            if (userId == null)
            {
                return Json(new { success = false, count = 0, message = "User not authenticated" });
            }

            var cartItemCount = await _cartSvc.GetCartItemCountAsync(userId);

            return Json(new { success = true, count = cartItemCount });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRelatedGameToCart(int gameId)
        {
            var userId = _userHelper.GetUserIdClaim();

            // Add the game to the cart (assuming this returns CartDTO)
            var result = await _cartSvc.AddGameToCartAsync(userId, gameId, 1);

            // Check if the result is not null (indicating success)
            if (result != null)
            {
                // Optionally: Get updated cart details (like item count)
                var cartItemCount = await _cartSvc.GetCartItemCountAsync(userId);

                // Return JSON response indicating success
                return Json(new { success = true, cartItemCount = cartItemCount });
            }

            // Return JSON response indicating failure
            return Json(new { success = false });
        }


    }
}
