using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using src.Helpers;

namespace Workout.Dashboard.Web.BusinessOps
{
    public class StrRateOperations : IStrRateOperations
    {
        public double CalculateAverageStrRate(IEnumerable<dynamic> exercises)
        {
            var a = exercises.Average(x => CalculateStrRate(x.lift, x.repcount));
            return 0;
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