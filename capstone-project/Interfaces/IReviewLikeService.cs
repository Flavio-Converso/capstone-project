namespace capstone_project.Interfaces
{
    public interface IReviewLikeService
    {
        Task LikeReviewAsync(int userId, int reviewId);
        Task UnlikeReviewAsync(int userId, int reviewId);
        Task<bool> HasUserLikedReviewAsync(int userId, int reviewId);
        Task<int> GetReviewLikeCountAsync(int reviewId);
    }

}
