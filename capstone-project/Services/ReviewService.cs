using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.ViewModels;

namespace capstone_project.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _ctx;

        public ReviewService(DataContext context)
        {
            _ctx = context;
        }

        public async Task CreateReviewAsync(ReviewViewModel reviewVm, int userId)
        {
            var review = new Review
            {
                Title = reviewVm.Title,
                Content = reviewVm.Content,
                Rating = reviewVm.Rating,
                GameId = reviewVm.GameId,
                UserId = userId,
                Date = DateTime.Now
            };

            _ctx.Reviews.Add(review);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> HasUserReviewedGameAsync(int userId, int gameId)
        {
            return await Task.FromResult(_ctx.Reviews.Any(r => r.UserId == userId && r.GameId == gameId));
        }
        public async Task LikeReviewAsync(int reviewId, int userId)
        {
            if (!await HasUserLikedReviewAsync(reviewId, userId))
            {
                var reviewLike = new ReviewLike
                {
                    ReviewId = reviewId,
                    UserId = userId
                };
                _ctx.ReviewLikes.Add(reviewLike);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task UnlikeReviewAsync(int reviewId, int userId)
        {
            var reviewLike = _ctx.ReviewLikes.FirstOrDefault(rl => rl.ReviewId == reviewId && rl.UserId == userId);
            if (reviewLike != null)
            {
                _ctx.ReviewLikes.Remove(reviewLike);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> HasUserLikedReviewAsync(int reviewId, int userId)
        {
            return await Task.FromResult(_ctx.ReviewLikes.Any(rl => rl.ReviewId == reviewId && rl.UserId == userId));
        }

        public async Task<int> GetReviewLikeCountAsync(int reviewId)
        {
            return await Task.FromResult(_ctx.ReviewLikes.Count(rl => rl.ReviewId == reviewId));
        }
    }
}
