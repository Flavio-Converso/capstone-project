using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterService _masterSvc;
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleSvc;

        public MasterController(IMasterService masterService, IUserService userService, IRoleService roleSvc)
        {
            _masterSvc = masterService;
            _userSvc = userService;
            _roleSvc = roleSvc;
        }

        public async Task<IActionResult> Dashboard()
        {
            var users = await _userSvc.GetAllUsersAsync();
            var roles = await _roleSvc.GetAllRolesAsync();

            var viewModel = new MasterViewModel
            {
                Users = users,
                Roles = roles,
                NewRole = new Role { Name = string.Empty }
            };

            return View(viewModel);
        }

        // Delete User
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userSvc.DeleteUserAsync(id);
            return RedirectToAction("Dashboard");
        }

        // Create Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Dashboard");
            }

            await _roleSvc.CreateRoleAsync(role);
            return RedirectToAction("Dashboard");
        }

        // Update Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            await _roleSvc.UpdateRoleAsync(role);
            return RedirectToAction("Dashboard");
        }

        // Delete Role
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleSvc.DeleteRoleAsync(id);
            return RedirectToAction("Dashboard");
        }

        // Add Role to User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleToUser(int userId, int roleId)
        {
            await _masterSvc.AddRoleToUserAsync(userId, roleId);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            var viewModel = new MasterViewModel
            {
                Users = await _userSvc.GetAllUsersAsync(),
                Roles = await _roleSvc.GetAllRolesAsync()
            };

            try
            {
                await _masterSvc.RemoveRoleFromUserAsync(userId, roleId);
                return RedirectToAction("Dashboard");
            }
            catch (InvalidOperationException ex)
            {
                // Add the error message to ModelState
                ModelState.AddModelError(string.Empty, ex.Message);

                // Return the same view with the error
                return View("Dashboard", viewModel);
            }
        }

    }
}
