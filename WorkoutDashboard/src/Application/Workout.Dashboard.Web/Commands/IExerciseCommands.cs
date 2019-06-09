using System.Threading.Tasks;
using System;

namespace Workout.Dashboard.Web.Commands.Inferfaces
{
    public interface IExerciseCommands
    {
        Task<decimal> GetTopStrRateInPeriod(int exerciseId, DateTime startDate, DateTime endDate);
    }
}