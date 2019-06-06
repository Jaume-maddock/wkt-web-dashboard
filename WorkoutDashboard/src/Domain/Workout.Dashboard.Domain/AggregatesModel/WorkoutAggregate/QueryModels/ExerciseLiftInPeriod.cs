using System;
using System.Collections.Generic;

namespace Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels
{
    public class ExerciseLiftsInPeriod
    {
        public DateTime StartDate { get; }
        
        public DateTime EndDate { get; }
        
        public ICollection<ExerciseLift> Exercises { get; set; }

        public ExerciseLiftsInPeriod(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            Exercises = new List<ExerciseLift>();
        }
    }

    public class ExerciseLift
    {

        public int TypeId { get; }

        public string TypeName { get; }
        
        public int Id { get; }
        
        public string Name { get; }
        
        public string Lift { get; private set; }
        
        public int RepCount { get; private set; }
        
        public int LiftTypeId { get; private set; }

        public ExerciseLift(int id, string name, int typeId, string typeName)
        {
            Id = id;
            Name = name;
            TypeId = typeId;
            TypeName = typeName;
        }

        public void SetWorkout(string lift, int repCount, int liftTypeId)
        {
            Lift = lift;
            RepCount = repCount;
            LiftTypeId = liftTypeId;
        }
    }
}