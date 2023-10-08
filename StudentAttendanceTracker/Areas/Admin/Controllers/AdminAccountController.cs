//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Controllers
{
    /// <summary>
    /// Account controller for administrators. Handles logging in for administrators
    /// </summary>
    [Route("[area]/{action}")]
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        /// <summary>
        /// AdminAccountController constructor to initialize private UserManager, SignInManager, and RoleManager objects.
        /// </summary>
        /// <param name="userMngr">UserManager object</param>
        /// <param name="signInMngr">SignInManager Object</param>
        /// <param name="roleMgr">RoleManager object</param>
        public AdminAccountController(UserManager<User> userMngr, SignInManager<User> signInMngr, RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMngr;
            roleManager = roleMgr;
            signInManager = signInMngr;
        }

        /// <summary>
        /// Get method that sees if user is already logged in and redirects them to their appropriate page if so, otherwise brings them to the faculty login page
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity is null || !signInManager.IsSignedIn(User))
                return View(new LoginViewModel());

            string role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

            return role switch
            {
                "Admin" => RedirectToAction("Index", "User"),
                "Student" => RedirectToAction("Home", "Student", new { area = "Student" }),
                "Professor" => RedirectToAction("Home", "Professor", new { area = "Faculty" }),
                "QualifiedStaff" => RedirectToAction("Home", "QualifiedStaff", new { area = "Faculty" }),
                _ => View(new LoginViewModel()),
            };
        }

        /// <summary>
        /// Post method that attempts to sign user in and check to make sure they are a site administrator
        /// </summary>
        /// <param name="model">LoginViewModel parameter that is assigned when user submits login request</param>
        /// <returns>Login page if unsuccessful, admin index page</returns>

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByNameAsync(model.Email);
                if (user is null)
                {
                    ViewBag.ErrorMessage = "Invalid Email or Password";
                    return View(model);
                }

                var roles = await userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    ViewBag.ErrorMessage = "Please sign into the appropriate page. ";
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "User");

                }
            }

            ViewBag.ErrorMessage = "Invalid Email or Password";
            return View(model);

        }

    }
}
