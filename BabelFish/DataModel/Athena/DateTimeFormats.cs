﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena
{
    [Obsolete( "Use Scopos.BabelFish.Helpers.DateTimeFormats instead.")]
    public static class DateTimeFormats
    {

        //NOTE: These values should be exactly the same as found in Medea.WebServices.Constants
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string DATETIME_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK";
    }
}