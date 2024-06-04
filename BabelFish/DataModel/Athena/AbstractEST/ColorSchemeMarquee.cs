using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeMarquee
    {
        public ColorSchemeMarquee()
        {
            /*
            Text[0] = "#ececec";
            Background[0] = "#232529";
            */
        }

        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Text color of the marquee.")]
        public String Text { set; get; } = "#ececec";

        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Background color of the marquee.")]
        public String Background { set; get; } = "#232529";
    }
}
