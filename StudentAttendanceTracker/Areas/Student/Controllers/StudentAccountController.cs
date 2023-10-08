//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Controllers
{
    /// <summary>
    /// Account controller for students. Handles logging in for students
    /// </summary>
    [Route("[area]/{action}")]
    [Area("Student")]
    public class StudentAccountController : Controller
    {
        private readonly AttendanceTrackerContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        /// <summary>
        /// StudentAccountController constructor to initialize private AttendanceTrackerContext, UserManager, SignInManager, and RoleManager objects.
        /// </summary>
        /// <param name="ctx">AttendanceTrackerContext object</param>
        /// <param name="userMngr">UserManager object</param>
        /// <param name="signInMngr">SignInManager Object</param>
        /// <param name="roleMgr">RoleManager object</param>
        public StudentAccountController(AttendanceTrackerContext ctx,UserManager<User> userMngr, SignInManager<User> signInMngr, RoleManager<IdentityRole> roleMgr)
        {
            context = ctx;
            userManager = userMngr;
            roleManager = roleMgr;
            signInManager = signInMngr;
        }

        /// <summary>
        /// Get method that sees if user is already logged in and redirects them to their appropriate page if so, otherwise brings them to student login page
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity is null || !signInManager.IsSignedIn(User))
                return View(new LoginViewModel ());

            string role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

            return role switch
            {
                "Admin" => RedirectToAction("Index", "User", new { area = "Admin" }),
                "Student" => RedirectToAction("Home", "Student"),
                "Professor" => RedirectToAction("Home", "Professor", new { area = "Faculty" }),
                "QualifiedStaff" => RedirectToAction("Home", "QualifiedStaff", new { area = "Faculty" }),
                _ => View(new LoginViewModel()),
            };
        }

        /// <summary>
        /// Post method that attempts to sign user in and check to make sure they are a student
        /// </summary>
        /// <param name="model">LoginViewModel parameter that is assigned when user submits login request</param>
        /// <returns>Login page if unsuccessful or the student home page</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if(user is null)
                {
                    ViewBag.ErrorMessage = "Invalid Email or Password";
                    return View(model);
                }

                var roles = await userManager.GetRolesAsync(user);
                if (!roles.Contains("Student"))
                {

                    ViewBag.ErrorMessage = "Please sign into the appropriate page. ";
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Home", "Student");
                }
            }
            ViewBag.ErrorMessage = "Invalid Email or Password";

            return View(model);

        }


    }
}
