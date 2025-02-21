using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// Text color of the marquee.
        /// </summary>
        [JsonPropertyName( "text" )]
        public String Text { set; get; } = "#ececec";

        /// <summary>
        /// Background color of the marquee.
        /// </summary>
        [JsonPropertyName( "background" )]
        public String Background { set; get; } = "#232529";
    }
}
