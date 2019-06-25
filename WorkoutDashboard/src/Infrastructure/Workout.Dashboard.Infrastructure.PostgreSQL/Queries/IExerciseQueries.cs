using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workout.Dashboard.Web.Queries
{
    public interface IExerciseQueries
    {
        Task<IEnumerable<IDictionary<string, object>>> GetExerciseExecutionsInPeriodAsync(int exerciseId, DateTime startDate, DateTime endDate); 
        Task<IEnumerable<IDictionary<string, object>>> GetLastExerciseExecutionAsync(int exerciseId);
        Task<IDictionary<string, object>> GetTopLiftInPeriodByExercise(int exerciseId, DateTime startDate, DateTime endDate);
        Task<IDictionary<string, object>> GetBasicInfo(int exerciseId);
    }
}