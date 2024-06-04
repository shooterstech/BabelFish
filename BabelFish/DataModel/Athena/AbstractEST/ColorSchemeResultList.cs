using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeResultList
    {
        public ColorSchemeResultList()
        {
            /*
            BottomLanes[0] = "#5c5c5c";
            TopLanes[0] = "#5d84bc";
            Flash[0] = "#bdbdbd";
            TopLanesText[0] = "#ececec";
            BottomLanesText[0] = "#ececec";
            */
        }

        [JsonProperty("bottomlanes", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Bottom lanes of the result list.")]
        public String BottomLanes { set; get; } = "#5c5c5c";

        [JsonProperty("toplanes", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Top 3 lanes of the result list.")]
        public String TopLanes { set; get; } = "#39598A";

        [JsonProperty("flash", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Flash color when updating the result list.")]
        public String Flash { set; get; } = "#bdbdbd";

        [JsonProperty("toplanestext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Top 3 lanes text color.")]
        public String TopLanesText { set; get; } = "#ececec";

        [JsonProperty("bottomlanestext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Bottom lanes text color.")]
        public String BottomLanesText { set; get; } = "#ececec";
    }
}
