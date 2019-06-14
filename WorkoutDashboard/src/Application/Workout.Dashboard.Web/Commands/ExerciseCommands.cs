using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Dashboard.Web.Queries;
using Workout.Dashboard.Web.BusinessOps;
using System.Dynamic;

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
            IEnumerable<IDictionary<string, object>> executions = await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate);
            return _strRateOperations.CalculateTopStrRateWithDate(executions);
        }

    }
}