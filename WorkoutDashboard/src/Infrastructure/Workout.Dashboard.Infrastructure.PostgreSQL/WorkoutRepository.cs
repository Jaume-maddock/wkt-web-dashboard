using System;
using System.Collections.Generic;
using Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Workout.Dashboard.Domain.AggregatesModel.WorkoutAggregate.QueryModels;

namespace Workout.Dashboard.Infrastructure.PostgreSQL
{
    /// <summary>
    /// Workout DB actions
    /// </summary>
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly IConfiguration _config;

        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="config"></param>
        public WorkoutRepository(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// DB Connection creation.
        /// </summary>
        private string _connection => (_config.GetConnectionString("PostgreConnectionString"));
        

        /// <summary>
        /// Retrieves top lift in a period from DB.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<TopLiftInPeriod> GetTopLiftInPeriod(DateTime startDate, DateTime endDate)
        {
            var sQuery = $@"select we.exercise_lift, w.workout_date from workout_exercises we
                                    inner join workout w on we.workout_id = w.workout_id
                                    where w.workout_date >= '{startDate}' and w.workout_date <= '{endDate}'
                                    order by we.exercise_lift desc limit 1;";

            /*using (var conn = new NpgsqlConnection(_connection))
            {
                conn.Open();
                var result = await conn.QueryAsync<TopLiftInPeriod>(sQuery, new { START_DATE = startDate, END_DATE = endDate });
                return result.FirstOrDefault();
            }*/
            
            NpgsqlConnection conn = new NpgsqlConnection(_connection);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(sQuery, conn);
            // Execute the query and obtain a result set
            var dr = await command.ExecuteReaderAsync();

            var result = new TopLiftInPeriod();
            while (dr.Read())
            {
                decimal.TryParse(dr[0].ToString(), out var lift);
                DateTime.TryParse(dr[1].ToString(), out var liftDate);
                result = new TopLiftInPeriod(lift, liftDate, liftDate);

            }
            conn.Close();
            return result;
        }
        
        /// <summary>
        /// Retrieves exercise count by muscle group
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<ExercisesByGroupInPeriod> GetNumberOfExercisesByGroupInPeriod(DateTime startDate, DateTime endDate)
        {
            var sQuery = $@"select COUNT(we.exercise_id) as count, ex.exercise_type_id, et.exercise_type_name from workout_exercises we
                        inner join exercise ex on we.exercise_id = ex.exercise_id
                        inner join exercise_type et on ex.exercise_type_id = et.exercise_type_id 
                        inner join workout w on we.workout_id = w.workout_id
                        where w.workout_date >= '{startDate}' and w.workout_date <= '{endDate}'
                        group by ex.exercise_type_id, et.exercise_type_name;";

            NpgsqlConnection conn = new NpgsqlConnection(_connection);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(sQuery, conn);
            // Execute the query and obtain a result set
            var dr = await command.ExecuteReaderAsync();

            var exercisesByGroup = new ExercisesByGroupInPeriod(startDate, endDate);
            while (dr.Read())
            {
                
                int.TryParse(dr["count"].ToString(), out var count);
                int.TryParse(dr["exercise_type_id"].ToString(), out var typeId);
                var group = new GroupExerciseCount(typeId, dr["exercise_type_name"].ToString(), count);
                exercisesByGroup.GroupExercises.Add(group);
            }
            conn.Close();
            return exercisesByGroup;
        }
        
        /// <summary>
        /// Retrieves total number of workouts in period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<WorkoutCountInPeriod> GetNumberOfWorkoutsInPeriod(DateTime startDate, DateTime endDate)
        {
            var sQuery = $@"select COUNT(w.workout_id) from workout w 
                        where w.workout_date >= '{startDate}' and w.workout_date <= '{endDate}'";

            NpgsqlConnection conn = new NpgsqlConnection(_connection);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(sQuery, conn);
            // Execute the query and obtain a result set
            var dr = await command.ExecuteReaderAsync();
            
            WorkoutCountInPeriod count = null;
            while (dr.Read())
            {
                int.TryParse(dr[0].ToString(), out var countNumber);
                count = new WorkoutCountInPeriod(countNumber, startDate, endDate);
            }
            conn.Close();
            return count;
        }

        /// <summary>
        /// Retrieves exercise count by muscle group
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<ExerciseLiftsInPeriod> GetExercisesAndLiftsByGroupInPeriod(DateTime startDate, DateTime endDate)
        {
            var sQuery = $@"select ex.exercise_type_id, et.exercise_type_name,
                        ex.exercise_id, ex.exercise_name, we.exercise_lift,
                        we.exercise_rep_count, we.lift_type_id
                        from workout_exercises we
                        inner join exercise ex on we.exercise_id = ex.exercise_id
                        inner join exercise_type et on ex.exercise_type_id = et.exercise_type_id 
                        inner join workout w on we.workout_id = w.workout_id
                        where w.workout_date >= '{startDate}' and w.workout_date <= '{endDate}';";

            NpgsqlConnection conn = new NpgsqlConnection(_connection);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(sQuery, conn);
            // Execute the query and obtain a result set
            var dr = await command.ExecuteReaderAsync();

            var exercisesByGroup = new ExerciseLiftsInPeriod(startDate, endDate);
            while (dr.Read())
            {
                
                int.TryParse(dr["exercise_type_id"].ToString(), out var exerciseTypeId);
                int.TryParse(dr["exercise_id"].ToString(), out var id);
                int.TryParse(dr["exercise_rep_count"].ToString(), out var repCount);
                int.TryParse(dr["lift_type_id"].ToString(), out var liftTypeId);
                var ex = new ExerciseLift(id, 
                    dr["exercise_name"].ToString(), exerciseTypeId, dr["exercise_type_name"].ToString());
                ex.SetWorkout(dr["exercise_lift"].ToString(), repCount, liftTypeId);
                exercisesByGroup.Exercises.Add(ex);
            }
            conn.Close();
            return exercisesByGroup;
        }
    }

    

}
