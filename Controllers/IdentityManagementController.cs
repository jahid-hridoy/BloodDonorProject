using BloodDonorProject.Models.ViewModel;
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
    }
}
