using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Dashboard.Web.Commands;

namespace Workout.Dashboard.Web.Controllers.Api
{
    [Route("api/v1/exercises")]
    [ApiController]
    public class ExerciseQueryController : Controller
    {
        private readonly IExerciseCommands _exerciseCommands;

        public ExerciseQueryController(IExerciseCommands exerciseCommands)
        {
            _exerciseCommands = exerciseCommands;
        }

        [Route("{id}/strrate/top")]
        [HttpGet]
        public async Task<IActionResult> GetTopStrRate(int id)
        {
            if(id <= 0) return BadRequest();
            return Ok(await GetTopStrRateInPeriod(id, DateTime.MinValue, DateTime.Now));
        }

        [Route("{id}/strrate/topindates")]
        [HttpGet]
        public async Task<IActionResult> GetTopStrRateInPeriod(int id, DateTime startDate, DateTime endDate)
        {
            if(id <= 0) return BadRequest();
            return Ok(await _exerciseCommands.GetTopStrRateInPeriod(id, startDate, endDate));
        }

        [Route("{id}/strrate/average")]
        [HttpGet]
        public async Task<IActionResult> GetAverageStrRate(int id)
        {
            if(id <= 0) return BadRequest();
            return Ok(await GetAverageStrRateInPeriod(id, DateTime.MinValue, DateTime.Now));
        }

        [Route("{id}/strrate/averageindates")]
        [HttpGet]
        public async Task<IActionResult> GetAverageStrRateInPeriod(int id, DateTime startDate, DateTime endDate)
        {
            if(id <= 0) return BadRequest();
            return Ok(await _exerciseCommands.GetAverageStrRateInPeriod(id, startDate, endDate));
        }

        [Route("{id}/strrate/current")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentStrRate(int id)
        {
            if(id <= 0) return BadRequest();
            return Ok(await _exerciseCommands.GetCurrentStrRate(id));
        }
    }
}