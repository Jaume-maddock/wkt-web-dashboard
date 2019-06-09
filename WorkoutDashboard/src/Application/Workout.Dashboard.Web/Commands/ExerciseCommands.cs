using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Dashboard.Web.Queries;
using Workout.Dashboard.Web.BusinessOps;
using Workout.Dashboard.Web.Commands.Inferfaces;

namespace Workout.Dashboard.Web.Commands
{
    public class ExerciseCommands// : IExerciseCommands
    {
        private readonly IExerciseQueries _exerciseQueries;
        private readonly IStrRateOperations _strRateOperations;
        public ExerciseCommands(IExerciseQueries exerciseQueries, IStrRateOperations strRateOperations)
        {
            _exerciseQueries = exerciseQueries;
            _strRateOperations = strRateOperations;
        }
        public async Task<decimal> GetTopStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate)
        {

            IEnumerable<IDictionary<string, object>> executions = await _exerciseQueries.GetExerciseExecutionsInPeriodAsync(exerciseId, startDate, endDate);
            return _strRateOperations.CalculateStrRateMultiple(executions);
        }
    }
}