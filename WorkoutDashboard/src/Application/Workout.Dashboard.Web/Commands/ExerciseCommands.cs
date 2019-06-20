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
        public async Task<decimal> GetAverageStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<IDictionary<string, object>> executions = await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate);
            return _strRateOperations.CalculateAverageStrRate(executions);
        }

        public async Task<decimal> GetCurrentStrRate(int exerciseId)
        {
            IEnumerable<IDictionary<string, object>> executions = await _exerciseQueries.GetLastExerciseExecutionAsync(exerciseId);
            return _strRateOperations.CalculateAverageStrRate(executions);
        }

        public async Task<dynamic> GetTopStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<dynamic> executions = (await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate)).AsDynamicList();
            var topExecution = _strRateOperations.CalculateTopStrRateExecution(executions);
            dynamic responseObject = new ExpandoObject();
            responseObject.StrRate = _strRateOperations.CalculateStrRate(topExecution.lift, topExecution.repcount);
            responseObject.Date = topExecution.workoutdate.ToShortDateString(); //TODO Add culture
            return responseObject;
        }

    }
}