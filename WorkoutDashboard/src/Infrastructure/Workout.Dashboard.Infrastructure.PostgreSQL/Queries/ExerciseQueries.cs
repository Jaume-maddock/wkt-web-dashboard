using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;

namespace Workout.Dashboard.Web.Queries
{
    public class ExerciseQueries : IExerciseQueries
    {
        /// <summary>
        /// Appsettings config
        /// </summary>
        private readonly IConfiguration _config;

        // <summary>
        /// DB Connection creation.
        /// </summary>
        private string _connectionString => (_config.GetConnectionString("PostgreConnectionString"));
        
        public ExerciseQueries(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Calculate top StrRate
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IDictionary<string, object>>> GetExerciseExecutionsInPeriodAsync(int exerciseId, DateTime startDate, DateTime endDate) 
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                IEnumerable<dynamic> dbResponse = await connection.QueryAsync<dynamic>(
                    $@"select ex.exercise_type_id as Type,
                        et.exercise_type_name as TypeName,
                        ex.exercise_id as _id,
                        ex.exercise_name as Name,
                        we.exercise_lift as Lift,
                        we.exercise_rep_count as RepCount,
                        we.lift_type_id as LiftType,
                        w.workout_date as WorkoutDate
                        from workout_exercises we
                        inner join exercise ex on we.exercise_id = ex.exercise_id
                        inner join exercise_type et on ex.exercise_type_id = et.exercise_type_id 
                        inner join workout w on we.workout_id = w.workout_id
                        where ex.exercise_id = '{exerciseId}'
                        and w.workout_date >= '{startDate}'
                        and w.workout_date <= '{endDate}';");
                return (from row in dbResponse select (IDictionary<string, object>)row).ToList();
            }
        } 

        /// <summary>
        /// Get last execution for an exercise. Can contain more than one row.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IDictionary<string, object>>> GetLastExerciseExecutionAsync(int exerciseId) 
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                IEnumerable<dynamic> dbResponse = await connection.QueryAsync<dynamic>(
                    $@"select ex.exercise_type_id as Type,
                        et.exercise_type_name as TypeName,
                        ex.exercise_id as _id,
                        ex.exercise_name as Name,
                        we.exercise_lift as Lift,
                        we.exercise_rep_count as RepCount,
                        we.lift_type_id as LiftType,
                        w.workout_date as WorkoutDate
                        from workout_exercises we
                        inner join exercise ex on we.exercise_id = ex.exercise_id
                        inner join exercise_type et on ex.exercise_type_id = et.exercise_type_id 
                        inner join workout w on we.workout_id = w.workout_id
                        where ex.exercise_id = '{exerciseId}'
                        order by w.workout_date desc
                        limit 1;");
                return (from row in dbResponse select (IDictionary<string, object>)row).ToList();
            }
        } 
    }
}