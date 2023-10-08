//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StudentAttendanceTracker.Models;

namespace StudentAttendanceTracker.Areas.Admin.Controllers
{
    /// <summary>
    /// Controls site functioning for the Admin section of the website
    /// </summary>
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        /// <summary>
        /// StudentController constructor to assign private AttendanceTrackerContext object
        /// </summary>
        /// <param name="userMngr">UserManager object</param>
        /// <param name="roleMngr">RoleManager object</param>
        public UserController(UserManager<User> userMngr, RoleManager<IdentityRole> roleMngr)
        {
            userManager = userMngr;
            roleManager = roleMngr;
        }

        /// <summary>
        /// displays all users and their roles in the index page of the admin section
        /// </summary>
        /// <returns>Index page of admin section</returns>
        public async Task<IActionResult> Index()
        {
            List<User> users = new();
            foreach (User user in userManager.Users)
            {
                user.RoleNames = await userManager.GetRolesAsync(user);
                users.Add(user);
            }
            UserViewModel model = new()
            {
                Users = users,
                Roles = roleManager.Roles
            };
            return View(model);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">UserId of user to delete</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User? user = await userManager.FindByIdAsync(id);
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

        /// <summary>
        /// Adds a user to a specified role
        /// </summary>
        /// <param name="id">Id of user to add</param>
        [HttpPost]
        public async Task<IActionResult> AddToRole(string id)
        {
            string role = Request.Form["AddToRole"].ToString() ?? "";
            IdentityRole? Role = await roleManager.FindByNameAsync(role);
            if (Role == null)
            {
                TempData["message"] = "role does not exist. ";
            }
            else
            {
                User? user = await userManager.FindByIdAsync(id);
             
                await userManager.AddToRoleAsync(user!, Role.Name!);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="id">UserId of the user to remove</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string id)
        {
            string role = Request.Form["RemoveFromRole"].ToString() ?? "";
            User? user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                TempData["message"] = "user does not exist";
            }
            await userManager.RemoveFromRoleAsync(user!, role);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="id">Name of role to delete</param>
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole? role = await roleManager!.FindByIdAsync(id);
            if (role is null)
            {
                TempData["message"] = "role does not exist. ";
            }
            else
            {
                await roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates a new role with the given name
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRole()
        {
            string role = Request.Form["NewRole"].ToString() ?? "";
            await roleManager.CreateAsync(new IdentityRole(role));
            return RedirectToAction("Index");
        }
    }
}
