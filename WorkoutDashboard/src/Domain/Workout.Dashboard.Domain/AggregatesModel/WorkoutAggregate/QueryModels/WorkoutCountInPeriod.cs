using System;

namespace Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels
{
    public class WorkoutCountInPeriod
    {
        public int Count { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        /// <summary>
        /// Ctor. Build a new object
        /// </summary>
        /// <param name="count"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public WorkoutCountInPeriod(int count, DateTime startDate, DateTime endDate)
        {
            Count = count;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}