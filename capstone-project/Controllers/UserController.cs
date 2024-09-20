using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userSvc;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _ctx;
        private readonly IWishlistService _wishlistSvc;
        public UserController(IUserService userService, IUserHelper userHelper, DataContext ctx, IWishlistService wishlistSvc)
        {
            _userSvc = userService;
            _userHelper = userHelper;
            _ctx = ctx;
            _wishlistSvc = wishlistSvc;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Get the current user ID
            int userId = _userHelper.GetUserIdClaim();

            // Get the user by their ID
            var user = await _userHelper.GetUserIdAsync(userId);

            // Get the games owned by the user via GameKey
            var ownedGames = await _ctx.GameKeys
                             .Where(gk => gk.UserId == userId)
                             .Include(gk => gk.Game)
                             .ThenInclude(g => g.GameImages)
                             .ToListAsync();

            var userCategoriesCount = user.Categories.Count;

            var wishlistItems = await _wishlistSvc.GetWishlistItemsAsync(userId);

            var userReviewsCount = await _ctx.Reviews
                             .Where(r => r.UserId == userId)
                             .CountAsync();

            // Populate the UserProfileViewModel
            var viewModel = new UserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                BirthDate = user.BirthDate,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                ZipCode = user.ZipCode,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender.ToString(),
                ProfileImg = user.ProfileImg,
                OwnedGames = ownedGames,
                UserCategoriesCount = userCategoriesCount,
                UserReviewsCount = userReviewsCount
            };

            ViewBag.ChangePasswordViewModel = new ChangePasswordViewModel();

            // Pass the wishlist items to the ViewBag
            ViewBag.WishlistItems = wishlistItems;

            return View(viewModel);
        }



        // GET: Load the current user's profile for editing
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {

            // Get the current user ID
            int userId = _userHelper.GetUserIdClaim();

            // Get the user's profile for editing
            var viewModel = await _userSvc.GetProfileForEditAsync(userId);

            return View(viewModel);
        }


        // POST: Save the edited profile data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(UserEditProfileViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Return the view with validation errors
            }

            try
            {
                // Get the current user ID
                int userId = _userHelper.GetUserIdClaim();

                // Update the profile via the AuthService
                await _userSvc.UpdateProfileAsync(userId, viewModel);

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                // Check if the exception message is related to the phone number being used
                if (ex.Message.Contains("in uso"))
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);  // Add the specific error to PhoneNumber
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiornamento del profilo."); // General error
                }

                return View(viewModel);
            }
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Handle password change
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            // Get the current user ID
            int userId = _userHelper.GetUserIdClaim();

            // Repopulate the necessary data for the Profile view
            var user = await _userHelper.GetUserIdAsync(userId);
            var ownedGames = await _ctx.GameKeys
                             .Where(gk => gk.UserId == userId)
                             .Include(gk => gk.Game)
                             .ThenInclude(g => g.GameImages)
                             .ToListAsync();
            var userCategoriesCount = user.Categories.Count;
            var wishlistItems = await _wishlistSvc.GetWishlistItemsAsync(userId);
            var userReviewsCount = await _ctx.Reviews
                                 .Where(r => r.UserId == userId)
                                 .CountAsync();

            // Populate the UserProfileViewModel
            var profileVm = new UserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                BirthDate = user.BirthDate,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                ZipCode = user.ZipCode,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender.ToString(),
                ProfileImg = user.ProfileImg,
                OwnedGames = ownedGames,
                UserCategoriesCount = userCategoriesCount,
                UserReviewsCount = userReviewsCount
            };

            if (!ModelState.IsValid)
            {
                // Pass the validation errors and set a flag to show the password section
                ViewBag.ChangePasswordViewModel = viewModel;
                ViewBag.WishlistItems = wishlistItems;
                ViewBag.ShowChangePasswordSection = true; // Flag to show the password section
                return View("Profile", profileVm); // Re-render the Profile view
            }

            try
            {
                // Call the AuthService to change the password
                await _userSvc.ChangePasswordAsync(userId, viewModel);

                // If successful, redirect to the Profile page
                return RedirectToAction("Profile", new { passwordChanged = true });
            }
            catch (Exception ex)
            {
                // Pass the exception message as a validation error
                ModelState.AddModelError("ChangePasswordError", ex.Message);

                // Pass the error and data back to the Profile view
                ViewBag.ChangePasswordViewModel = viewModel;
                ViewBag.WishlistItems = wishlistItems;
                ViewBag.ShowChangePasswordSection = true; // Flag to show the password section
                return View("Profile", profileVm); // Re-render the Profile view
            }
        }



        public async Task<IActionResult> GetProfileImage()
        {
            var base64Image = await _userHelper.GetProfileImageBase64Async();

            if (!string.IsNullOrEmpty(base64Image))
            {
                return Json(new { success = true, image = base64Image });
            }

            return Json(new { success = false, message = "No image found" });
        }


    }
}
