using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeImageDisplay
    {
        public ColorSchemeImageDisplay()
        {
            //Background[0] = "#5d84bc";
        }

        /// <summary>
        /// Background color of image screen
        /// </summary>
        [JsonPropertyName( "background" )]
        public String Background { set; get; } = "#5d84bc";
    }
}
