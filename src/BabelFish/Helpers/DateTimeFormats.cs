using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Scopos.BabelFish.Helpers {
    public static class DateTimeFormats {

        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string DATETIME_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK";
        public const string TIME_FORMAT = "hh:mm:ss.ffffff";

        //It's been observed that some older match documents are using 7 decimal places instead of six.
        //To parse correctly, defining a secondary format. Which should only be used for parsing, not for writing.
        public const string DATETIME_FORMAT_SECONDARY = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK";
        public const string TIME_FORMAT_SECONDARY = "hh:mm:ss.fffffff";

        //Used in calling DateTime.ParseExact() 
        public static CultureInfo CULTURE = CultureInfo.InvariantCulture;
	}
}
