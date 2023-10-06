using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Controllers
{
    [Route("[area]/{action}")]
    [Area("Faculty")]
    public class FacultyAccountController : Controller
    {
        private AttendanceTrackerContext context;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public FacultyAccountController(AttendanceTrackerContext ctx, UserManager<User> userMngr, SignInManager<User> signInMngr, RoleManager<IdentityRole> roleMgr)
        {
            context = ctx;
            userManager = userMngr;
            roleManager = roleMgr;
            signInManager = signInMngr;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity is null || !signInManager.IsSignedIn(User))
                return View(new LoginViewModel());

            string role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

            switch (role)
            {
                case "Admin":
                    return RedirectToAction("Index", "User", new { area = "Admin" });
                case "Student":
                    return RedirectToAction("Home", "Student", new { area = "Student" });
                case "Professor":
                    return RedirectToAction("Home", "Professor");
                case "QualifiedStaff":
                    return RedirectToAction("Home", "QualifiedStaff");
                default:
                    return View(new LoginViewModel());
            }

        }

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
                if (role != "Professor" && role != "QualifiedStaff")
                {
                    ViewBag.ErrorMessage = "Please sign into the appropriate page.";
                    return View(model);
                }
                
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return role == "Professor" ? RedirectToAction("Home", "Professor") : RedirectToAction("Home", "QualifiedStaff");
                }
            }
            ViewBag.ErrorMessage = "Invalid Email or Password";

            return View(model);

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

    }
}
