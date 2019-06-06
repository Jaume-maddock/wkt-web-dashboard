using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Helpers;
using Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate;
using Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels;

namespace Workout.Dashboard.Web.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QueryModelsController : Controller
    {
        private readonly IWorkoutRepository _workoutRepository;

        public QueryModelsController(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        /*[HttpGet]
        public async Task<TopLiftInPeriod> GetTopLiftInSlicedPeriod(DateTime startDate, DateTime endDate)
        {
            return await _workoutRepository.GetTopLiftInPeriod(startDate, endDate);
        }*/
        

        /// <summary>
        /// Slices a date period in equal parts and retrieves max lift during each sub-period 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="daysInSlice"></param>
        /// <returns></returns>
        [Route("GetTopLiftInSlicedPeriod")]
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<TopLiftInPeriod>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTopLiftInSlicedPeriod(DateTime startDate, DateTime endDate, int daysInSlice)
        {
            /* Validate input */
            if (endDate <= startDate.AddDays(daysInSlice))
            {
                return BadRequest($"No {daysInSlice} days period between {startDate} and {endDate}");
            }
            
            /* Separate period in slices and query them */
            var queries = new List<Task<TopLiftInPeriod>>();
            var currentDate = startDate;
            while (currentDate < endDate)
            {
                queries.Add(_workoutRepository.GetTopLiftInPeriod(currentDate, currentDate = currentDate.AddDays(daysInSlice)));
            }
            List<TopLiftInPeriod> liftInPeriods = (await Task.WhenAll(queries)).ToList();
            
            return Ok(liftInPeriods);
        }

        /// <summary>
        /// Retrieves the number of exercises by muscle group in a period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [Route("GetNumberOfExercisesByGroupInPeriod")]
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ExercisesByGroupInPeriod>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetNumberOfExercisesByGroupInPeriod(DateTime startDate, DateTime endDate)
        {
            /* Validate input */
            if (endDate <= startDate)
            {
                return BadRequest($"Start date {startDate} is equal or previous to {endDate}");
            }

            return Ok(await _workoutRepository.GetNumberOfExercisesByGroupInPeriod(startDate, endDate));
        }
        
        /// <summary>
        /// Gets number of workouts in period during each sub-period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="daysInSlice"></param>
        /// <returns></returns>
        [Route("GetNumberOfWorkoutsInSlicedPeriod")]
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<WorkoutCountInPeriod>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetNumberOfWorkoutsInSlicedPeriod(DateTime startDate, DateTime endDate, int daysInSlice)
        {
            /* Validate input */
            if (endDate <= startDate.AddDays(daysInSlice))
            {
                return BadRequest($"No {daysInSlice} days period between {startDate} and {endDate}");
            }
            
            /* Separate period in slices and query them */
            var queries = new List<Task<WorkoutCountInPeriod>>();
            var currentDate = startDate;
            while (currentDate < endDate)
            {
                queries.Add(_workoutRepository.GetNumberOfWorkoutsInPeriod(currentDate, currentDate = currentDate.AddDays(daysInSlice)));
            }
            List<WorkoutCountInPeriod> workoutCount = (await Task.WhenAll(queries)).ToList();
            
            return Ok(workoutCount);
        }
        
        /// <summary>
        /// Gets STRRate for a given period, separated in slices
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="daysInSlice"></param>
        /// <returns></returns>
        [Route("GetStrRateByGroupInPeriod")]
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<StrRateInPeriod>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetStrRateByGroupInPeriod(DateTime startDate, DateTime endDate, int daysInSlice)
        {
            /* Validate input */
            if (endDate <= startDate.AddDays(daysInSlice))
            {
                return BadRequest($"No {daysInSlice} days period between {startDate} and {endDate}");
            }
            
            /* Separate period in slices and query them */
            var queries = new List<Task<ExerciseLiftsInPeriod>>();
            var currentDate = startDate;
            while (currentDate < endDate)
            {
                queries.Add(_workoutRepository.GetExercisesAndLiftsByGroupInPeriod(currentDate, currentDate = currentDate.AddDays(daysInSlice)));
            }
            List<ExerciseLiftsInPeriod> periodsWithExercises = (await Task.WhenAll(queries)).ToList();

            var strRates = new List<GroupStrRate>();

            var exerciseTypes = periodsWithExercises.SelectMany(pe => pe.Exercises.Select(ex => new {ex.TypeId, ex.TypeName } )).Distinct().ToList();
            
            if (!exerciseTypes.Any()) return NoContent();
            foreach (var exerciseType in exerciseTypes)
            {
                var exerciseGroup = new GroupStrRate(exerciseType.TypeId, exerciseType.TypeName);
                foreach (var period in periodsWithExercises)
                {
                    var exercisesOfTypeInPeriod = period.Exercises.Where(e => e.TypeId == exerciseType.TypeId).ToList();
                    if(!exercisesOfTypeInPeriod.Any()) continue;
                    var groupStrRate = decimal.Round(exercisesOfTypeInPeriod.Average(x => x.Lift.AsDecimal() * x.RepCount), 2);
                    var strRateInPeriod = new StrRateInPeriod(groupStrRate, period.StartDate, period.EndDate);
                    exerciseGroup.StrRateInPeriods.Add(strRateInPeriod);
                }
                strRates.Add(exerciseGroup);
            } 
            return Ok(strRates);
        }
    }
}