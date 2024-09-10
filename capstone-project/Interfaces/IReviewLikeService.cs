namespace capstone_project.Interfaces
{
    public interface IReviewLikeService
    {
        Task<bool> LikeReviewAsync(int reviewId, int userId);
        Task<int> GetLikeCountAsync(int reviewId);


    }
}
