using capstone_project.Models.ViewModels;

namespace capstone_project.Interfaces
{
    public interface IReviewService
    {
        Task<int> CreateReviewAsync(ReviewViewModel reviewVm, int userId);
        Task<bool> HasUserReviewedGameAsync(int userId, int gameId);
        Task DeleteReviewAsync(int reviewId, int userId); // New delete method

    }
}
