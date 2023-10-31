using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Converters {

    public class DateConverter  : IsoDateTimeConverter {

        public DateConverter() {
            base.DateTimeFormat = DateTimeFormats.DATE_FORMAT;
        }
    }

    public class DateTimeConverter : IsoDateTimeConverter {

        public DateTimeConverter() {
            base.DateTimeFormat = DateTimeFormats.DATETIME_FORMAT;
        }
    }

    public class TimeConverter : IsoDateTimeConverter {

        public TimeConverter() {
            base.DateTimeFormat = DateTimeFormats.TIME_FORMAT;
        }
    }
}
