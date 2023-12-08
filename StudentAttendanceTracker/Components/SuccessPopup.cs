//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.AspNetCore.Mvc;

namespace StudentAttendanceTracker.Components
{
    public class SuccessPopup : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
