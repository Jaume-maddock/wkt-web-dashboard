using Microsoft.AspNetCore.Mvc;

namespace Workout.Dashboard.Web.Controllers.Api
{
    [Route("exercises")]
    public class ExerciseController : Controller
    {
        [Route("{exerciseId}")]
        public IActionResult Index([FromRoute]int exerciseId)
        {
            ViewBag.ExerciseId = exerciseId;
            return View();
        }
    }
}