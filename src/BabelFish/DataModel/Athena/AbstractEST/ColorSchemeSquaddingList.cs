using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// Squadding lanes color.
        /// </summary>
        [JsonPropertyName( "squaddinglanes" )]
        public String SquaddingLanes { set; get; } = "#5d84bc";

        /// <summary>
        /// Squadding lanes text color.
        /// </summary>
        [JsonPropertyName( "squaddinglanestext" )]
        public String SquaddingLanesText { set; get; } = "#ececec";
    }
}
