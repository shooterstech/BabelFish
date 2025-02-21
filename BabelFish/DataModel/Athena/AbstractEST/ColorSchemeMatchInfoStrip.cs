using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeMatchInfoStrip
    {
        public ColorSchemeMatchInfoStrip()
        {
            /*
            TopStrip[0] = "#232529";
            BottomStrip[0] = "#232529";
            TopText[0] = "#ececec";
            BottomText[0] = "#ececec";
            */
        }

        /// <summary>
        /// Very top strip background of the match info.
        /// </summary>
        [JsonPropertyName( "topstrip" )]
        public String TopStrip { set; get; } = "#232529";

        /// <summary>
        /// Bottom strip background of the match info.
        /// </summary>
        [JsonPropertyName( "bottomstrip" )]
        public String BottomStrip { set; get; } = "#232529";

        /// <summary>
        /// Top strip text color.
        /// </summary>
        [JsonPropertyName( "toptext" )]
        public String TopText { set; get; } = "#ececec";

        /// <summary>
        /// Bottom strip text color.
        /// </summary>
        [JsonPropertyName( "bottomtext" )]
        public String BottomText { set; get; } = "#ececec";
    }
}
