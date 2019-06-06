using System;

namespace Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels
{
    /// <summary>
    /// Top lift by month
    /// </summary>
    public class TopLiftInPeriod
    {
        public decimal MaxLift { get; }

        public DateTime StartDate { get; }
        
        public DateTime EndDate { get; }

        /// <summary>
        /// Ctor. Build a new object
        /// </summary>
        /// <param name="maxLift"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public TopLiftInPeriod(decimal maxLift, DateTime startDate, DateTime endDate)
        {
            MaxLift = maxLift;
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Void ctor for materialization (PostgreSQL)
        /// </summary>
        public TopLiftInPeriod()
        {
            
        }
    }
}