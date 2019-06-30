using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workout.Dashboard.Web.Commands
{
    public interface IExerciseCommands
    {
        Task<dynamic> GetAverageStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
        Task<dynamic> GetTopStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
        Task<dynamic> GetCurrentStrRate(int exerciseId);
        Task<dynamic> GetTopLiftInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<dynamic>> GetStrRateEvolutionInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
        Task<dynamic> GetExerciseBasicInfo(int exerciseId);
        Task<IEnumerable<dynamic>> GetExerciseTree();
    }
}