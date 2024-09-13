using capstone_project.Models;
using capstone_project.Models.DTOs.Auth;

namespace capstone_project.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserRegisterDTO dto);
        Task<User> LoginAsync(UserLoginDTO dto);

    }
}
