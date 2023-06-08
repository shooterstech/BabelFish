using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {
    public static class DateTimeFormats {

        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string DATETIME_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK";
        public const string TIME_FORMAT = "hh\\:mm\\:ss\\.ffffff";

        /// <summary>
        /// Formats a span of dates, a start and end date, to a common formatted string.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string FormatSpanOfDates( DateTime startDate, DateTime endDate ) {
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
    }
}
