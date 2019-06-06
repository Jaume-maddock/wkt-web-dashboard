using System;
using System.Threading.Tasks;
using Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels;

namespace Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate
{
    public interface IWorkoutRepository
    {
        Task<TopLiftInPeriod> GetTopLiftInPeriod(DateTime startDate, DateTime endDate);
        Task<ExercisesByGroupInPeriod> GetNumberOfExercisesByGroupInPeriod(DateTime startDate, DateTime endDate);
        Task<WorkoutCountInPeriod> GetNumberOfWorkoutsInPeriod(DateTime startDate, DateTime endDate);
        Task<ExerciseLiftsInPeriod> GetExercisesAndLiftsByGroupInPeriod(DateTime startDate, DateTime endDate);

    }
}