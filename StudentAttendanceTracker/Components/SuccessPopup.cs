using Microsoft.AspNetCore.Mvc;

namespace StudentAttendanceTracker.Components
{
    public class SuccessPopup : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
