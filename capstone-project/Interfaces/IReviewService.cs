using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IReviewService
    {
        Task<Review?> CreateReviewAsync(ReviewDTO comment, int userId);
    }
}
