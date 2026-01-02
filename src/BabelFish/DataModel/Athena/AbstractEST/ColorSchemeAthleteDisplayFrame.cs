using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// Athlete background color
        /// </summary>
        [JsonPropertyName( "athletebg" )]
        public String AthleteBG { set; get; } = "#5d84bc";

        /// <summary>
        /// Athlete text color.
        /// </summary>
        [JsonPropertyName( "athletetext" )]
        public String AthleteText { set; get; } = "#ececec";

        /// <summary>
        /// Score background color
        /// </summary>
        [JsonPropertyName( "scorebg" )]
        public String ScoreBG { set; get; } = "#5c5c5c";

        /// <summary>
        /// Score text color
        /// </summary>
        [JsonPropertyName( "scoretext" )]
        public String ScoreText { set; get; } = "#ececec";

        /// <summary>
        /// Aggregate background color.
        /// </summary>
        [JsonPropertyName( "aggbg" )]
        public String AggBG { set; get; } = "#5c5c5c";

        /// <summary>
        /// Aggregate text color.
        /// </summary>
        [JsonPropertyName( "aggtext" )]
        public String AggText { set; get; } = "#ececec";
    }
}
