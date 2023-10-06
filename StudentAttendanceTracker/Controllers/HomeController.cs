using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Diagnostics;
using System.Security.Principal;

namespace StudentAttendanceTracker.Controllers
{
	public class HomeController : Controller
	{
		

		private SignInManager<User> signInManager;

		public HomeController(SignInManager<User> mngr) => signInManager = mngr;

		public IActionResult Index()
		{
			return View();
		}

        public async Task<IActionResult> SignOut()
        {
			
			await signInManager.SignOutAsync();
			HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
			HttpContext.Response.Cookies.Delete(".AspNetCore.Identity");
			return RedirectToAction("Index");
        }

    }
}