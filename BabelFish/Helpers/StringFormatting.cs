using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {
    /// <summary>
    /// Provides helper functions to format certain data into common string formatting
    /// </summary>
    public static class StringFormatting {

        /// <summary>
        /// Formats a span of dates, a start and end date, to a common formatted string.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string SpanOfDates( DateTime startDate, DateTime endDate ) {
            if (startDate == endDate)
                return startDate.ToString( "ddd, dd MMM yyyy" );
            else if (startDate.Year == endDate.Year
                && startDate.Month == endDate.Month)
                return $"{startDate.ToString( "dd" )} - {endDate.ToString( "dd MMM yyyy" )}";
            else if (startDate.Year == endDate.Year)
                return $"{startDate.ToString( "dd MMM" )} - {endDate.ToString( "dd MMM yy" )}";
            else
                return $"{startDate.ToString( "MM/dd/yy" )} - {endDate.ToString( "MM/dd/yy" )}";

        }

        public static string Location( string city, string state, string country="" ) {
            if (country == "" || country == "USA") {
                //In the USA
                if (!string.IsNullOrEmpty( city ) && ! string.IsNullOrEmpty( state ))
                    return $"{city}, {state}";
                if (string.IsNullOrEmpty( state ))
                    return city;
                if (string.IsNullOrEmpty( city ))
                    return state;
                return "";
            } else {
                //Outside the USA
                if (!string.IsNullOrEmpty( city ))
                    return $"{city}, {country}";

                if (!string.IsNullOrEmpty( state ))
                    return $"{state}, {country}";
                return country;
            }
        }
    }
}
