using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Dashboard.Web.Queries;
using Workout.Dashboard.Web.BusinessOps;
using System.Dynamic;
using Workout.Dashboard.Web.Helpers;

namespace Workout.Dashboard.Web.Commands
{
    public class ExerciseCommands : IExerciseCommands
    {   
        private readonly IExerciseQueries _exerciseQueries;
        private readonly IStrRateOperations _strRateOperations;
        public ExerciseCommands(IExerciseQueries exerciseQueries, IStrRateOperations strRateOperations)
        {
            _exerciseQueries = exerciseQueries;
            _strRateOperations = strRateOperations;
        }

        /// <summary>
        /// Calculates the average Str-Rate in a period for an exercise.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<dynamic> GetAverageStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<dynamic> executions = (await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate)).AsDynamicList();
            dynamic responseObject = new ExpandoObject();
            responseObject.StrRate = _strRateOperations.CalculateAverageStrRate(executions);
            return responseObject;
        }

        /// <summary>
        /// Calculates current Str-Rate based on all executions from last workout session.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public async Task<dynamic> GetCurrentStrRate(int exerciseId)
        {
            IEnumerable<dynamic> lastWorkoutAllExecutions = (await _exerciseQueries.GetLastExerciseExecutionAsync(exerciseId)).AsDynamicList();
            if(!lastWorkoutAllExecutions.Any()) return null;
            
            dynamic responseObject = new ExpandoObject();
            var topExecutionOneDay = _strRateOperations.CalculateTopStrRateExecution(lastWorkoutAllExecutions);
            responseObject.StrRate = _strRateOperations.CalculateStrRate(topExecutionOneDay.lift, topExecutionOneDay.repcount);
            //responseObject.StrRate = _strRateOperations.CalculateAverageStrRate(lastWorkoutAllExecutions);
            responseObject.Date = lastWorkoutAllExecutions.First().workoutdate.ToShortDateString(); //TODO Add culture
            return responseObject;
        }

        /// <summary>
        /// Calculates the top Str-Rate in a period.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTopStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<dynamic> executions = (await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate)).AsDynamicList();
            var topExecution = _strRateOperations.CalculateTopStrRateExecution(executions);
            dynamic responseObject = new ExpandoObject();
            responseObject.StrRate = _strRateOperations.CalculateStrRate(topExecution.lift, topExecution.repcount);
            responseObject.Date = topExecution.workoutdate.ToShortDateString(); //TODO Add culture
            return responseObject;
        }

        /// <summary>
        /// Gets top lift, repcount and date in a period.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<dynamic> GetTopLiftInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {
            dynamic topLiftExecution = (await _exerciseQueries.GetTopLiftInPeriodByExercise(exerciseId, startDate, endDate)).AsDynamic();
            topLiftExecution.workoutdate = topLiftExecution.workoutdate.ToShortDateString(); //TODO add culture
            return topLiftExecution;
        }

        /// <summary>
        /// Get StrRate evolution.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetStrRateEvolutionInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<dynamic> executions = (await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate)).AsDynamicList();
            var dateExecutions = executions.GroupBy(x => x.workoutdate);

            var responseObjectList = new List<dynamic>(); 
            foreach (var executionGroup in dateExecutions)
            {
                dynamic oneDayData = new ExpandoObject();
                var topExecutionOneDay = _strRateOperations.CalculateTopStrRateExecution(executionGroup);
                oneDayData.StrRate = _strRateOperations.CalculateStrRate(topExecutionOneDay.lift, topExecutionOneDay.repcount);
                oneDayData.Date = executionGroup.First().workoutdate.ToShortDateString();
                responseObjectList.Add(oneDayData);
            }
            return responseObjectList;
        }

        /// <summary>
        /// Get basic info.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public async Task<dynamic> GetExerciseBasicInfo(int exerciseId)
        {
            return (await _exerciseQueries.GetBasicInfo(exerciseId)).AsDynamic();
        }
    }
}