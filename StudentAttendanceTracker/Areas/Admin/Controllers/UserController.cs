using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StudentAttendanceTracker.Models;

namespace StudentAttendanceTracker.Areas.Admin.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<User> userManager; private RoleManager<IdentityRole> roleManager;
        public UserController(UserManager<User> userMngr, RoleManager<IdentityRole> roleMngr)
        {
            userManager = userMngr;
            roleManager = roleMngr;
        }

        
        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();
            foreach (User user in userManager.Users)
            {
                user.RoleNames = await userManager.GetRolesAsync(user);
                users.Add(user);
            }
            UserViewModel model = new UserViewModel
            {
                Users = users,
                Roles = roleManager.Roles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                { // if failed
                    string errorMessage = "";
                    foreach (IdentityError error in result.Errors)
                    {
                        errorMessage += error.Description + " | ";
                    }
                    TempData["message"] = errorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        // the Add() methods work like the Register() methods from
        [HttpPost]
        public async Task<IActionResult> AddToRole(string id)
        {
            string role = Request.Form["AddToRole"].ToString() ?? "";
            IdentityRole Role = await roleManager.FindByNameAsync(role);
            if (Role == null)
            {
                TempData["message"] = "role does not exist. ";
            }
            else
            {
                User user = await userManager.FindByIdAsync(id);
                await userManager.AddToRoleAsync(user, Role.Name);
            }
            return RedirectToAction("Home");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string id)
        {
            string role = Request.Form["RemoveFromRole"].ToString() ?? "";
            User user = await userManager.FindByIdAsync(id);
            await userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction("Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole()
        {
            string role = Request.Form["NewRole"].ToString() ?? "";
            await roleManager.CreateAsync(new IdentityRole(role));
            return RedirectToAction("Home");
        }
    }
}
