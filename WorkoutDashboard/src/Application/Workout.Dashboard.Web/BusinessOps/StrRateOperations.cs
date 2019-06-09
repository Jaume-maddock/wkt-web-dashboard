using System.Collections.Generic;
using System.Linq;
using src.Helpers;

namespace Workout.Dashboard.Web.BusinessOps
{
    public class StrRateOperations : IStrRateOperations
    {
        public decimal CalculateStrRateMultiple(IEnumerable<IDictionary<string, object>> exercises)
        {
            return exercises.Average(x => x["lift"].ToString().AsDecimal(2) * x["repcount"].ToString().AsDecimal(1));
        }
    }
}