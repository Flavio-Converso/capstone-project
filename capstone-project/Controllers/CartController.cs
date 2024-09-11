using capstone_project.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace capstone_project.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartSvc;
        public CartController(ICartService cartService)
        {
            _cartSvc = cartService;
        }

        // GET: /Game/Cart
        public async Task<IActionResult> Cart()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!);
            var cart = await _cartSvc.GetCartByUserIdAsync(userId);
            return View(cart);
        }

        // POST: /Game/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int gameId, string source)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!);
            await _cartSvc.AddGameToCartAsync(userId, gameId, 1);

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

        // POST: /Game/RemoveFromCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int gameId, string source)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!);
            await _cartSvc.RemoveGameFromCartAsync(userId, gameId);
            return RedirectToAction("Cart");

        }

        // POST: /Game/UpdateCartItemQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItemQuantity(int gameId, int quantity)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!);

            if (quantity < 1)
            {
                quantity = 1;
            }

            await _cartSvc.UpdateCartItemQuantityAsync(userId, gameId, quantity);
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Checkout()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!);
            var cart = await _cartSvc.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Cart");
            }

            var successUrl = Url.Action("CheckoutSuccess", null, Request.Scheme);

            var checkoutUrl = await _cartSvc.CreateCheckoutSessionAsync(cart, successUrl!);

            return Redirect(checkoutUrl);
        }

        public async Task<IActionResult> CheckoutSuccess()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!);

            await _cartSvc.CompleteCheckoutAsync(userId);

            return View();
        }
    }
}
