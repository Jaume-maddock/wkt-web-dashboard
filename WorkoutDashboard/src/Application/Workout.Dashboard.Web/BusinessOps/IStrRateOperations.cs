using System.Collections.Generic;

namespace Workout.Dashboard.Web.BusinessOps
{
    public interface IStrRateOperations
    {
        decimal CalculateStrRateMultiple(IEnumerable<IDictionary<string, object>> exercises);
    }
}