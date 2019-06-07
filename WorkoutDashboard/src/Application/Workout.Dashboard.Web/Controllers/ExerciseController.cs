using Microsoft.AspNetCore.Mvc;

namespace Workout.Dashboard.Web.Controllers.Api
{
    public class ExerciseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}