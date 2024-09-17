using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewSvc;
        private readonly IUserHelper _userHelper;
        private readonly IReviewLikeService _reviewLikeSvc;
        public ReviewController(IReviewService reviewService, IReviewLikeService reviewLikeService, IUserHelper userHelper)
        {
            _reviewSvc = reviewService;
            _userHelper = userHelper;
            _reviewLikeSvc = reviewLikeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel reviewVm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Game", new { id = reviewVm.GameId });
            }

            var userId = _userHelper.GetUserIdClaim();

            await _reviewSvc.CreateReviewAsync(reviewVm, userId);

            return RedirectToAction("Details", "Game", new { id = reviewVm.GameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int reviewId, int gameId)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _reviewSvc.DeleteReviewAsync(reviewId, userId);

            return RedirectToAction("Details", "Game", new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeReview(int reviewId)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _reviewLikeSvc.LikeReviewAsync(userId, reviewId);

            var likeCount = await _reviewLikeSvc.GetReviewLikeCountAsync(reviewId);
            return Json(new { success = true, likeCount });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlikeReview(int reviewId)
        {
            var userId = _userHelper.GetUserIdClaim();
            await _reviewLikeSvc.UnlikeReviewAsync(userId, reviewId);

            var likeCount = await _reviewLikeSvc.GetReviewLikeCountAsync(reviewId);
            return Json(new { success = true, likeCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewLikeCount(int reviewId)
        {
            var likeCount = await _reviewLikeSvc.GetReviewLikeCountAsync(reviewId);
            return Json(new { likeCount });
        }

    }
}
