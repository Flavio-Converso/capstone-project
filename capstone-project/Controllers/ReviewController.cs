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

        public ReviewController(IReviewService reviewService, IUserHelper userHelper)
        {
            _reviewSvc = reviewService;
            _userHelper = userHelper;
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
        public async Task<IActionResult> LikeReview(int reviewId)
        {
            var userId = _userHelper.GetUserIdClaim();
            bool hasLiked = await _reviewSvc.HasUserLikedReviewAsync(reviewId, userId);

            if (hasLiked)
            {
                await _reviewSvc.UnlikeReviewAsync(reviewId, userId);
            }
            else
            {
                await _reviewSvc.LikeReviewAsync(reviewId, userId);
            }

            // Get the updated like count
            var likeCount = await _reviewSvc.GetReviewLikeCountAsync(reviewId);

            // Return JSON response
            return Json(new { success = true, liked = !hasLiked, likeCount = likeCount });
        }
    }
}
