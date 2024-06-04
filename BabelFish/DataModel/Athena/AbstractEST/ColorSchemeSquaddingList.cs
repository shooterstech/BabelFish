using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeSquaddingList
    {
        public ColorSchemeSquaddingList()
        {
            /*
            SquaddingLanes[0] = "#5d84bc";
            SquaddingLanesText[0] = "#ececec";
            */
        }

        [JsonProperty("squaddinglanes", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Squadding lanes color.")]
        public String SquaddingLanes { set; get; } = "#5d84bc";

        [JsonProperty("squaddinglanestext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Squadding lanes text color.")]
        public String SquaddingLanesText { set; get; } = "#ececec";
    }
}
