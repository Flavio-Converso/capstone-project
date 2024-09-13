using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _ctx;

        public RoleService(DataContext dataContext)
        {
            _ctx = dataContext;
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _ctx.Roles.ToListAsync();
        }
        public async Task<Role> CreateRoleAsync(Role role)
        {
            _ctx.Roles.Add(role);
            await _ctx.SaveChangesAsync();
            return role;
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            var role = await _ctx.Roles.FindAsync(roleId);
            return role!;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            _ctx.Roles.Update(role);
            await _ctx.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _ctx.Roles.FindAsync(roleId);
            _ctx.Roles.Remove(role!);
            await _ctx.SaveChangesAsync();
            return true;
        }

    }
}
