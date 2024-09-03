namespace capstone_project.Interfaces.Auth
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}

