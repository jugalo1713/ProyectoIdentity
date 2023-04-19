using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoIdentity.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManagaer;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            this._userManagaer = userManager;
        }
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteUser(string email)
        {
            if (string.IsNullOrEmpty(email)) return View("Error");

            var user = await _userManagaer.FindByEmailAsync(email);

            if (user == null) return View("Error");

            var roles = await _userManagaer.GetRolesAsync(user);

            if (roles.Any())
                await _userManagaer.RemoveFromRolesAsync(user, roles);

            var result = await _userManagaer.DeleteAsync(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
