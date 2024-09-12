using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _dataContext;

        public ReviewService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Review?> CreateReviewAsync(ReviewDTO dto, int userId)
        {
            if (dto == null)
            {
                return null;
            }
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            var game = await _dataContext.Games.FirstOrDefaultAsync(g => g.GameId == dto.GameId);
            var review = new Review
            {
                User = user!,
                Game = game!,
                Title = dto.Title,
                Content = dto.Content,
                Rating = dto.Rating,
                Date = DateTime.Now,
                GameId = dto.GameId,
            };
            await _dataContext.AddAsync(review);
            await _dataContext.SaveChangesAsync();
            return review;
        }
    }
}
