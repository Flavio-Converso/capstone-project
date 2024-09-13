namespace capstone_project.Interfaces
{
    public interface IMasterService
    {
        Task<bool> AddRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);

    }

}
