using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        public async Task DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _ctx.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);

            if (review != null)
            {
                _ctx.Reviews.Remove(review);
                await _ctx.SaveChangesAsync();
            }
        }

    }
}
