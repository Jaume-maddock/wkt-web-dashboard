using System;
using System.Threading.Tasks;

namespace Workout.Dashboard.Web.Commands
{
    public interface IExerciseCommands
    {
        Task<dynamic> GetAverageStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
        Task<dynamic> GetTopStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
        Task<dynamic> GetCurrentStrRate(int exerciseId);
    }
}