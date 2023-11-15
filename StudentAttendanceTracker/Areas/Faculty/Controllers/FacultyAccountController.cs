//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models.Identity;
using StudentAttendanceTracker.Models.Initialization;
using StudentAttendanceTracker.Models.ViewModels;
using System.Security.Claims;

namespace StudentAttendanceTracker.Controllers
{
    /// <summary>
    /// Account controller for Instructors and qualified staff. Handles logging in for Instructors and qualified staff
    /// </summary>
    [Route("[area]/{action}")]
    [Area("Faculty")]
    public class FacultyAccountController : Controller
    {
        private readonly AttendanceTrackerContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        /// <summary>
        /// FacultyAccountController constructor to initialize private AttendanceTrackerContext, UserManager, SignInManager, and RoleManager objects.
        /// </summary>
        /// <param name="ctx">AttendanceTrackerContext object</param>
        /// <param name="userMngr">UserManager object</param>
        /// <param name="signInMngr">SignInManager Object</param>
        /// <param name="roleMgr">RoleManager object</param>
        public FacultyAccountController(AttendanceTrackerContext ctx, UserManager<User> userMngr, SignInManager<User> signInMngr, RoleManager<IdentityRole> roleMgr)
        {
            context = ctx;
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
                "Admin" => RedirectToAction("Home", "Admin", new { area = "Admin" }),
                "Student" => RedirectToAction("Home", "Student", new { area = "Student" }),
                "Instructor" => RedirectToAction("Home", "Instructor"),
                "QualifiedStaff" => RedirectToAction("Home", "QualifiedStaff"),
                _ => View(new LoginViewModel()),
            };
        }

        /// <summary>
        /// Post method that attempts to sign user in and check to make sure they are a Instructor or qualified staff memeber
        /// </summary>
        /// <param name="model">LoginViewModel parameter that is assigned when user submits login request</param>
        /// <returns>Login page if unsuccessful, qualified staff home page, or Instructor home page</returns>

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
                string role = roles[0];
                if (role != "Instructor" && role != "QualifiedStaff")
                {
                    ViewBag.ErrorMessage = "Please sign into the appropriate page.";
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return role == "Instructor" ? RedirectToAction("Home", "Instructor") : RedirectToAction("Home", "QualifiedStaff");
                }
            }
            ViewBag.ErrorMessage = "Invalid Email or Password";

            return View(model);

        }

    }
}
