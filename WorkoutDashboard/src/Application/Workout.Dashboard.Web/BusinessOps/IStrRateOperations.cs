using System.Collections.Generic;

namespace Workout.Dashboard.Web.BusinessOps
{
    public interface IStrRateOperations
    {
        decimal CalculateAverageStrRate(IEnumerable<IDictionary<string, object>> exercises);
        decimal CalculateStrRate(decimal lift, int repCount);
        decimal CalculateTopStrRate(IEnumerable<IDictionary<string, object>> exercises);
        dynamic CalculateTopStrRateWithDate(IEnumerable<IDictionary<string, object>> exercises);
    }
}