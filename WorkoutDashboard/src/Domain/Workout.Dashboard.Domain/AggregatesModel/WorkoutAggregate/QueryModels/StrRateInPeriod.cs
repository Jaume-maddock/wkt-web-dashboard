using System;
using System.Collections.Generic;

namespace Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels
{
    public class StrRateInPeriod
    {
        public decimal StrRate { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }
        
        public StrRateInPeriod(decimal strRate, DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            StrRate = strRate;
        }
    }

    public class GroupStrRate
    {
        public ICollection<StrRateInPeriod> StrRateInPeriods { get; set; }
        public int ExerciseTypeId { get; }
        public string ExerciseTypeName { get; }

        public GroupStrRate(int typeId, string typeName)
        {
            StrRateInPeriods = new List<StrRateInPeriod>();
            ExerciseTypeId = typeId;
            ExerciseTypeName = typeName;
        }
    }
    
}