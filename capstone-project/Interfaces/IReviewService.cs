using capstone_project.Models.ViewModels;

namespace capstone_project.Interfaces
{
    public interface IReviewService
    {
        Task CreateReviewAsync(ReviewViewModel reviewVm, int userId);
        Task<bool> HasUserReviewedGameAsync(int userId, int gameId);
        Task LikeReviewAsync(int reviewId, int userId);
        Task UnlikeReviewAsync(int reviewId, int userId);
        Task<bool> HasUserLikedReviewAsync(int reviewId, int userId);
        Task<int> GetReviewLikeCountAsync(int reviewId);
    }
}
