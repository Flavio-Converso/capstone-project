namespace capstone_project.Helpers
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}

