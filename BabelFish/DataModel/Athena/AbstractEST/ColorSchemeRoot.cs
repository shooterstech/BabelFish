using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeRoot
    {
        public ColorSchemeRoot()
        {
            /*
            NoViewsText[0] = "#ececec";
            NoViewsBG[0] = "#5d84bc";
            */
        }

        [JsonProperty("noviewstext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Text color with no views.")]
        public String NoViewsText { get; set; } = "#ececec";

        [JsonProperty("noviewsbg", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Background color with no views.")]
        public String NoViewsBG { get; set; } = "#5d84bc";
    }
}
