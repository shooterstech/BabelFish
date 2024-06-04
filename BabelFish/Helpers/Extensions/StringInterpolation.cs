using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Helpers.Extensions
{
    public static class StringInterpolation
    {
        /// <summary>
        /// Performs string interpolation using a dictionary for the source of the values. 
        /// </summary>
        /// <param name="source">The string to replace values with. All values must be in {}.</param>
        /// <param name="values">The dictionary, that holds the name value pairs for replacement.</param>
        /// <returns></returns>
        public static string Replace(this string source, Dictionary<string, string> values)
        {
            return values.Aggregate(
                source,
                (current, parameter) => current
                    .Replace($"{{{parameter.Key}}}", parameter.Value));
        }
    }
}
