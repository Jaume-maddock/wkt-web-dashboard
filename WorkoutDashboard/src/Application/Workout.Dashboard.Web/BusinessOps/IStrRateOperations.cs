using System.Collections.Generic;

namespace Workout.Dashboard.Web.BusinessOps
{
    public interface IStrRateOperations
    {
        decimal CalculateAverageStrRate(IEnumerable<dynamic> exercises);
        decimal CalculateStrRate(decimal lift, int repCount);
        dynamic CalculateTopStrRateExecution(IEnumerable<dynamic> executions);

    }
}