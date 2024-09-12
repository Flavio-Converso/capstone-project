using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _ctx;
        private readonly IUserHelper _userHelper;
        public ReviewService(DataContext dataContext, IUserHelper userHelper)
        {
            _ctx = dataContext;
            _userHelper = userHelper;
        }
        public async Task<Review?> CreateReviewAsync(ReviewDTO dto, int userId)
        {
            if (dto == null)
            {
                return null;
            }
            var user = await _userHelper.GetUserIdAsync(userId);
            var game = await _ctx.Games.FirstOrDefaultAsync(g => g.GameId == dto.GameId);
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
            await _ctx.AddAsync(review);
            await _ctx.SaveChangesAsync();
            return review;
        }
    }
}
