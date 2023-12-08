//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models.Identity;

namespace StudentAttendanceTracker.Controllers
{
    /// <summary>
    /// Controls site functioning for the student section of the website
    /// </summary>
    public class HomeController : Controller
	{
		

		private readonly SignInManager<User> signInManager;
        /// <summary>
        /// HomeController constructor to assign private SignInManager object
        /// </summary>
        /// <param name="mngr">SignInManager object</param>
        public HomeController(SignInManager<User> mngr) => signInManager = mngr;

        /// <summary>
        /// Displays the index page of the website
        /// </summary>
		public IActionResult Index()
		{
			return View();
		}

        /// <summary>
        /// Logs the currently signed in user out and deletes their cookies.
        /// </summary>
        public async Task<IActionResult> LogOut()
        {
			
			await signInManager.SignOutAsync();
			HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
			HttpContext.Response.Cookies.Delete(".AspNetCore.Identity");
			return RedirectToAction("Index");
        }

    }
}