using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeAthleteDisplayFrame
    {
        public ColorSchemeAthleteDisplayFrame()
        {
            /*
            AthleteBG[0] = "#5d84bc";
            AthleteText[0] = "#ececec";
            ScoreBG[0] = "#5c5c5c";
            ScoreText[0] = "#ececec";
            AggBG[0] = "#5c5c5c";
            AggText[0] = "#ececec";
            */
        }

        [JsonProperty("athletebg", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Athlete background color.")]
        public String AthleteBG { set; get; } = "#5d84bc";

        [JsonProperty("athletetext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Athlete text color.")]
        public String AthleteText { set; get; } = "#ececec";

        [JsonProperty("scorebg", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Score background color.")]
        public String ScoreBG { set; get; } = "#5c5c5c";

        [JsonProperty("scoretext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Score text color.")]
        public String ScoreText { set; get; } = "#ececec";

        [JsonProperty("aggbg", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Aggregate background color.")]
        public String AggBG { set; get; } = "#5c5c5c";

        [JsonProperty("aggtext", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Aggregate text color.")]
        public String AggText { set; get; } = "#ececec";
    }
}
