using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.BouncyCastle.Asn1;

namespace Scopos.BabelFish.Helpers.Extensions
{
    public static class StringInterpolation
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Performs string interpolation using a dictionary for the source of the values. 
        /// </summary>
        /// <param name="source">The string to replace values with. All values must be in {}.</param>
        /// <param name="values">The dictionary, that holds the name value pairs for replacement.</param>
        /// <returns></returns>
        public static string Replace(this string source, Dictionary<string, string> values)
        {
            try {
                if (string.IsNullOrEmpty( source ))
                    return string.Empty;

                return values.Aggregate(
                    source,
                    ( current, parameter ) => current
                        .Replace( $"{{{parameter.Key}}}", parameter.Value ) );
            } catch (Exception e) {
                _logger.Error( $"Could not interpolate {source}." );
                return string.Empty;
            }
        }

        /// <summary>
        /// Inputs a string such as "{XXXX}" and returns the portion of the 
        /// string that's inside the curly brackets. Wich in this example
        /// would be "XXXX".
        /// If the input string does not start and end with curly brackets
        /// then an empty string is returned.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ExtractFieldValue(this string source) {
            //Remove the curly braces and return the inner value.
            if (source.StartsWith( "{" ) && source.EndsWith( "{" ))
                return source.Substring( 1, source.Length - 2 );

            return string.Empty;
        }
    }
}
