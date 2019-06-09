using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Dashboard.Web.Commands;

namespace Workout.Dashboard.Web.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExerciseQueryController : Controller
    {
        private readonly ExerciseCommands _exerciseCommands;

        public ExerciseQueryController(ExerciseCommands exerciseCommands)
        {
            _exerciseCommands = exerciseCommands;
        }

        [Route("{id}/topstr")]
        [HttpGet]
        public async Task<IActionResult> GetTopStrRate(int id)
        {
            if(id <= 0) return BadRequest();
            return Ok(await _exerciseCommands.GetTopStrRateInPeriod(id, DateTime.MinValue, DateTime.Now));
        }
    }
}