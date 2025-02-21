using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// Text color with no views.
        /// </summary>
        [JsonPropertyName("noviewstext")]
        [Description("Text color with no views.")]
        public String NoViewsText { get; set; } = "#ececec";

        /// <summary>
        /// Background color with no views.
        /// </summary>
        [JsonPropertyName("noviewsbg")]
        public String NoViewsBG { get; set; } = "#5d84bc";
    }
}
