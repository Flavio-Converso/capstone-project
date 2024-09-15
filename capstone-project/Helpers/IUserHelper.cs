using capstone_project.Models;

namespace capstone_project.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserIdAsync(int userId);
        int GetUserIdClaim();
        Task<string?> GetProfileImageBase64Async();
    }
}
