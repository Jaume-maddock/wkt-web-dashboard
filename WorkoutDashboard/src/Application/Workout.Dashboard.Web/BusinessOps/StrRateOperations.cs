using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using src.Helpers;

namespace Workout.Dashboard.Web.BusinessOps
{
    public class StrRateOperations : IStrRateOperations
    {
        public decimal CalculateAverageStrRate(IEnumerable<IDictionary<string, object>> exercises)
        {
            return exercises.Average(x => CalculateStrRate(x["lift"].ToString().AsDecimal(2), x["repcount"].ToString().AsInt()));
        }

        public decimal CalculateStrRate(decimal lift, int repCount)
        {
            return lift * repCount;
        }

        public decimal CalculateTopStrRate(IEnumerable<IDictionary<string, object>> exercises)
        {
            return exercises.Max(x => CalculateStrRate(x["lift"].ToString().AsDecimal(2), x["repcount"].ToString().AsInt()));
        }

        public dynamic CalculateTopStrRateWithDate(IEnumerable<IDictionary<string, object>> exercises)
        {
            var top = exercises.Aggregate((e1 ,e2) => CalculateStrRate(e1["lift"].ToString().AsDecimal(2), e1["repcount"].ToString().AsInt())
            > CalculateStrRate(e2["lift"].ToString().AsDecimal(2), e2["repcount"].ToString().AsInt()) ? e1 : e2);
            var responseObject = new ExpandoObject();
            responseObject.StrRate = CalculateStrRate(top["lift"].ToString().AsDecimal(2), top["repcount"].ToString().AsInt());
            responseObject.Date = top["WorkoutDate"];
            return responseObject;
        }
    }
}