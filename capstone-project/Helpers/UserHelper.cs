using capstone_project.Data;
using capstone_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace capstone_project.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _ctx;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserIdAsync(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            return user!;
        }
        public int GetUserIdClaim()
        {
            var userIdClaim = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Redirect("/Auth/Login");
                throw new InvalidOperationException("User ID is not available or invalid.");
            }
        }
        public async Task<string?> GetProfileImageBase64Async()
        {
            var userId = GetUserIdClaim();
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user?.ProfileImg != null)
            {
                return Convert.ToBase64String(user.ProfileImg);
            }

            return null;
        }

    }
}
