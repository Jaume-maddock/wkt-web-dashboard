using Microsoft.AspNetCore.Mvc;

namespace Workout.Dashboard.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("/Views/Dashboard/Index.cshtml");
        }

        public IActionResult Overview()
        {
            return View("/Views/Dashboard/Overview.cshtml");
        }
    }
}