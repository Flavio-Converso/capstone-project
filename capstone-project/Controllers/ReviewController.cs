using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using capstone_project.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewSvc;
        private readonly IReviewLikeService _reviewLikeSvc;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _ctx;

        public ReviewController(IReviewService reviewService, IReviewLikeService reviewLikeService, IUserHelper userHelper, DataContext dataContext)
        {
            _reviewSvc = reviewService;
            _reviewLikeSvc = reviewLikeService;
            _userHelper = userHelper;
            _ctx = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewDTO dto)
        {
            var userId = _userHelper.GetUserIdClaim();

            if (!ModelState.IsValid)
            {
                // Return to the Game Details view with the validation errors.
                var game = await _ctx.Games.FindAsync(dto.GameId);
                var viewModel = new GameDetailsViewModel
                {
                    Game = game!,
                    Review = dto // Pass the invalid ReviewDTO back for validation errors
                };
                return View("Details", viewModel);
            }

            await _reviewSvc.CreateReviewAsync(dto, userId);
            return RedirectToAction("Details", "Game", new { id = dto.GameId });
        }


        [HttpPost]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var userId = _userHelper.GetUserIdClaim();

            var result = await _reviewLikeSvc.LikeReviewAsync(id, userId);

            var likeCount = await _reviewLikeSvc.GetLikeCountAsync(id);

            return Json(new { likeCount });
        }



        [HttpGet]
        public async Task<IActionResult> GetLikeCount(int id)
        {
            var likeCount = await _reviewLikeSvc.GetLikeCountAsync(id);

            return Json(new { likeCount });
        }

    }
}
