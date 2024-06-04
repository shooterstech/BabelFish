using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorSchemeImageDisplay
    {
        public ColorSchemeImageDisplay()
        {
            //Background[0] = "#5d84bc";
        }

        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Populate)]
        [Description("Background color of image screen.")]
        public String Background { set; get; } = "#5d84bc";
    }
}
