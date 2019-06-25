using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using src.Helpers;

namespace Workout.Dashboard.Web.BusinessOps
{
    public class StrRateOperations : IStrRateOperations
    {
        public decimal CalculateAverageStrRate(IEnumerable<dynamic> exercises)
        {
            decimal strRateSum = 0;
            foreach (var item in exercises)
            {
                strRateSum += CalculateStrRate(item.lift, item.repcount);
            }
            var result = strRateSum / exercises.Count();
            return decimal.Round(result, 2);
            //TODO return exercises.Average(x => (decimal)CalculateStrRate(x.lift, x.repcount));
        }

        public decimal CalculateStrRate(decimal lift, int repCount)
        {
            return lift * repCount;
        }
        
        public dynamic CalculateTopStrRateExecution(IEnumerable<dynamic> executions)
        {
            var top = executions.Aggregate((e1 ,e2) => CalculateStrRate(e1.lift, e1.repcount)
            > CalculateStrRate(e2.lift, e2.repcount) ? e1 : e2);
            return top;
        }
    }
}