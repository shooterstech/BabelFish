using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Scopos.BabelFish.Helpers {
    public static class DateTimeFormats {

        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string DATETIME_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK";
        public const string TIME_FORMAT = "hh\\:mm\\:ss\\.ffffff";

        //Used in calling DateTime.ParseExact()
		public static CultureInfo CULTURE = CultureInfo.InvariantCulture;
	}
}
