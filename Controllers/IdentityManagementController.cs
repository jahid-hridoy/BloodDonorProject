using BloodDonorProject.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonorProject.Controllers
{
    public class IdentityManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();

            var userWithRoles = users.Select(user => new UserWithRolesVIewModel
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Roles = _userManager.GetRolesAsync(user).Result.ToList()
            }).ToList();

            return View(userWithRoles);
        }

        public async Task<IActionResult> ManageUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User Id Not Found");
            }
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                AvailableRoles = roles,
                UserRoles = userRoles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRolesAsync(string userId, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User Id Not Found");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, roles);
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // POST: Create new role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Error"] = "Role name cannot be empty.";
                return RedirectToAction("Roles");
            }

            var existing = await _roleManager.RoleExistsAsync(roleName);
            if (existing)
            {
                TempData["Error"] = "Role already exists.";
                return RedirectToAction("Roles");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
                ? $"Role '{roleName}' created successfully."
                : "Failed to create role.";

            return RedirectToAction("Roles");
        }

        // POST: Delete a role if not in use
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                TempData["Error"] = "Role not found.";
                return RedirectToAction("Roles");
            }

            if (roleName == "Admin")
            {
                TempData["Error"] = "Cannot delete protected role 'Admin'.";
                return RedirectToAction("Roles");
            }

            // Check if any users have this role
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (usersInRole.Any())
            {
                TempData["Error"] = $"Cannot delete role '{roleName}' because it is assigned to users.";
                return RedirectToAction("Roles");
            }

            var result = await _roleManager.DeleteAsync(role);
            TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
                ? $"Role '{roleName}' deleted successfully."
                : "Failed to delete role.";

            return RedirectToAction("Roles");
        }
    }
}
