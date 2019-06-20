using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Workout.Dashboard.Web.Helpers
{
    public static class DictionaryExtensions
    {
        public static dynamic AsDynamic(this IDictionary<string, object> dictionary)
        {
            dynamic eo = dictionary.Aggregate(new ExpandoObject() as IDictionary<string, object>,
                            (a, p) => { a.Add(p.Key, p.Value); return a; });
            return eo;
        }

        public static IEnumerable<dynamic> AsDynamicList(this IEnumerable<IDictionary<string, object>> dictionaryList)
        {
            var dynExecutions = new List<dynamic>();
            foreach (var item in dictionaryList)
            {
               dynExecutions.Add(item.AsDynamic()); 
            }

            return dynExecutions;
        }
    }
}