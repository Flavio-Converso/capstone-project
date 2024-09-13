using capstone_project.Models;
using capstone_project.Models.ViewModels;

namespace capstone_project.Interfaces
{
    public interface IUserService
    {
        Task<bool> DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<UserEditProfileViewModel> GetProfileForEditAsync(int userId);
        Task UpdateProfileAsync(int userId, UserEditProfileViewModel viewModel);
        Task ChangePasswordAsync(int userId, ChangePasswordViewModel viewModel);

    }
}
