using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class ReviewLikeService : IReviewLikeService
    {
        private readonly DataContext _ctx;

        public ReviewLikeService(DataContext context)
        {
            _ctx = context;
        }

        public async Task LikeReviewAsync(int userId, int reviewId)
        {
            if (!await HasUserLikedReviewAsync(userId, reviewId))
            {
                var reviewLike = new ReviewLike
                {
                    UserId = userId,
                    ReviewId = reviewId
                };

                _ctx.ReviewLikes.Add(reviewLike);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task UnlikeReviewAsync(int userId, int reviewId)
        {
            var reviewLike = await _ctx.ReviewLikes
                .FirstOrDefaultAsync(rl => rl.UserId == userId && rl.ReviewId == reviewId);

            if (reviewLike != null)
            {
                _ctx.ReviewLikes.Remove(reviewLike);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> HasUserLikedReviewAsync(int userId, int reviewId)
        {
            return await _ctx.ReviewLikes
                .AnyAsync(rl => rl.UserId == userId && rl.ReviewId == reviewId);
        }

        public async Task<int> GetReviewLikeCountAsync(int reviewId)
        {
            return await _ctx.ReviewLikes.CountAsync(rl => rl.ReviewId == reviewId);
        }
    }
}
