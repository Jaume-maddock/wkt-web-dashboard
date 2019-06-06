using System;
using System.Collections;
using System.Collections.Generic;

namespace Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels
{
    public class ExercisesByGroupInPeriod
    {
        public ICollection<GroupExerciseCount> GroupExercises { get; set; }
        
        public DateTime StartDate { get; }
        
        public DateTime EndDate { get; }
        
        public ExercisesByGroupInPeriod(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            GroupExercises = new List<GroupExerciseCount>();
        }
    }


    public class GroupExerciseCount
    {
        public int GroupCode { get; }
        public string GroupName { get; }
        public int Count { get; }

        public GroupExerciseCount(int groupCode, string groupName, int count)
        {
            GroupCode = groupCode;
            GroupName = groupName;
            Count = count;
        }
    }
}