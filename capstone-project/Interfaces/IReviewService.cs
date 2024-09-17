using capstone_project.Models.ViewModels;

namespace capstone_project.Interfaces
{
    public interface IReviewService
    {
        Task CreateReviewAsync(ReviewViewModel reviewVm, int userId);
        Task<bool> HasUserReviewedGameAsync(int userId, int gameId);
        Task DeleteReviewAsync(int reviewId, int userId); // New delete method

    }
}
