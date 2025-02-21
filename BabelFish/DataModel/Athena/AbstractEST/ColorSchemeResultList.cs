using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// Bottom lanes of the result list.
        /// </summary>
        [JsonPropertyName( "bottomlanes" )]
        public String BottomLanes { set; get; } = "#5c5c5c";

        /// <summary>
        /// Top 3 lanes of the result list.
        /// </summary>
        [JsonPropertyName( "toplanes" )]
        public String TopLanes { set; get; } = "#39598A";

        /// <summary>
        /// Flash color when updating the result list.
        /// </summary>
        [JsonPropertyName( "flash" )]
        public String Flash { set; get; } = "#bdbdbd";

        /// <summary>
        /// Top 3 lanes text color.
        /// </summary>
        [JsonPropertyName( "toplanestext" )]
        public String TopLanesText { set; get; } = "#ececec";

        /// <summary>
        /// Bottom lanes text color.
        /// </summary>
        [JsonPropertyName( "bottomlanestext" )]
        public String BottomLanesText { set; get; } = "#ececec";
    }
}
