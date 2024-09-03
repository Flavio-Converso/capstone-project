using capstone_project.Interfaces.Auth;

namespace capstone_project.Services.Auth
{
    public class PasswordHelper : IPasswordHelper
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);

        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        }
    }
}
