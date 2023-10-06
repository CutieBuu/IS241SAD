using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Controllers
{
    [Route("[area]/{action}")]
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AdminAccountController(UserManager<User> userMngr, SignInManager<User> signInMngr, RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMngr;
            roleManager = roleMgr;
            signInManager = signInMngr;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity is null || !signInManager.IsSignedIn(User))
                return View(new LoginViewModel ());

            string role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

            switch (role)
            {
                case "Admin":
                    return RedirectToAction("Index", "User");
                case "Student":
                    return RedirectToAction("Home", "Student", new { area = "Student" });
                case "Professor":
                    return RedirectToAction("Home", "Professor", new { area = "Faculty" });
                case "QualifiedStaff":
                    return RedirectToAction("Home", "QualifiedStaff", new { area = "Faculty" });
                default:
                    return View(new LoginViewModel ());
            }

            
        }

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
