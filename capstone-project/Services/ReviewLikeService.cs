using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using Microsoft.EntityFrameworkCore;

public class ReviewLikeService : IReviewLikeService
{
    private readonly DataContext _dataContext;

    public ReviewLikeService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> LikeReviewAsync(int reviewId, int userId)
    {
        try
        {
            var review = await _dataContext.Reviews
                .Include(r => r.ReviewLikes)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null)
            {
                return false;
            }

            var existingLike = review.ReviewLikes
                .FirstOrDefault(rl => rl.UserId == userId);

            if (existingLike != null)
            {
                _dataContext.ReviewLikes.Remove(existingLike);
            }
            else
            {
                var user = _dataContext.Users.FirstOrDefault(u => u.UserId == userId);
                review.ReviewLikes.Add(new ReviewLike { Review = review, User = user });
            }

            await _dataContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LikeReviewAsync: {ex.Message}");
            return false;
        }
    }
    public async Task<int> GetLikeCountAsync(int reviewId)
    {
        var review = await _dataContext.Reviews
            .Include(r => r.ReviewLikes)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

        return review?.ReviewLikes.Count ?? 0;
    }
}