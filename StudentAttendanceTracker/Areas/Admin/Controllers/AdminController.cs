//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 https://github.com/vgc12
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using StudentAttendanceTracker.Models.ViewModels;
using StudentAttendanceTracker.Models.Identity;
using StudentAttendanceTracker.Models.Initialization;
using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceTracker.Areas.Admin.Controllers
{
    /// <summary>
    /// Controls site functioning for the Admin section of the website
    /// </summary>
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AttendanceTrackerContext _context;
        /// <summary>
        /// StudentController constructor to assign private AttendanceTrackerContext object
        /// </summary>
        /// <param name="userMngr">UserManager object</param>
        /// <param name="roleMngr">RoleManager object</param>
        public AdminController(UserManager<User> userMngr, RoleManager<IdentityRole> roleMngr, AttendanceTrackerContext context)
        {
            _userManager = userMngr;
            _roleManager = roleMngr;
            _context = context;
        }

        /// <summary>
        /// Displays the home page of the admin section of the site
        /// </summary>
        /// <returns>Admin/Admin/Home</returns>
        public IActionResult Home()
        {
            return View(_context.Admins.First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        /// <summary>
        /// displays all users and their roles in the index page of the admin section
        /// </summary>
        /// <returns>Index page of admin section</returns>
        public async Task<IActionResult> Index()
        {
            List<User> users = new();
            foreach (User user in _userManager.Users)
            {
                user.RoleNames = await _userManager.GetRolesAsync(user);
                users.Add(user);
            }
            UserViewModel model = new()
            {
                Users = users,
                Roles = _roleManager.Roles
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
            User? user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
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
            IdentityRole? Role = await _roleManager.FindByNameAsync(role);
            if (Role == null)
            {
                TempData["message"] = "role does not exist. ";
            }
            else
            {
                User? user = await _userManager.FindByIdAsync(id);
             
                await _userManager.AddToRoleAsync(user!, Role.Name!);
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
            User? user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                TempData["message"] = "user does not exist";
            }
            await _userManager.RemoveFromRoleAsync(user!, role);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="id">Name of role to delete</param>
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole? role = await _roleManager!.FindByIdAsync(id);
            if (role is null)
            {
                TempData["message"] = "role does not exist. ";
            }
            else
            {
                await _roleManager.DeleteAsync(role);
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
            await _roleManager.CreateAsync(new IdentityRole(role));
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ViewResult Report()
        {
            return View("~/Views/Shared/Report.cshtml", new ReportViewModel
            {
                Courses = _context.Courses.Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .Where(c => c.Students.Count > 0 && c.Instructor != null)
                        .OrderBy(c => c.CourseId)
                        .ToList(),
                Caller = "Admin"
            }) ; 

        }



    }
}
