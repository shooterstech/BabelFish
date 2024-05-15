using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        [JsonProperty("topstrip", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Very top strip background of the match info.")]
        public String TopStrip { set; get; } = "#232529";

        [JsonProperty("bottomstrip", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Bottom strip background of the match info.")]
        public String BottomStrip { set; get; } = "#232529";

        [JsonProperty("toptext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Top strip text color.")]
        public String TopText { set; get; } = "#ececec";

        [JsonProperty("bottomtext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Bottom strip text color.")]
        public String BottomText { set; get; } = "#ececec";
    }
}
