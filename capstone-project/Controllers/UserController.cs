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
        public UserController(IUserService userService, IUserHelper userHelper, DataContext ctx)
        {
            _userSvc = userService;
            _userHelper = userHelper;
            _ctx = ctx;
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
                OwnedGames = ownedGames
            };

            return View(viewModel);
        }


        // GET: Load the current user's profile for editing
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
                // Add the error message to the model state
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Handle password change
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Return the view with validation errors
            }

            try
            {
                // Get the current user ID
                int userId = _userHelper.GetUserIdClaim();

                // Call the AuthService to change the password
                await _userSvc.ChangePasswordAsync(userId, viewModel);

                // Redirect to profile or success page
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                // Add the error message (e.g., incorrect old password) to ModelState
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
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
