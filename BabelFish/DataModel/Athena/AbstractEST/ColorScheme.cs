using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ColorScheme
    {
        public ColorScheme()
        {
            Root = new ColorSchemeRoot();
            ResultList = new ColorSchemeResultList();
            SquaddingList = new ColorSchemeSquaddingList();
            MatchInfoStrip = new ColorSchemeMatchInfoStrip();
            Marquee = new ColorSchemeMarquee();
            ImageDisplay = new ColorSchemeImageDisplay();
            AthleteDisplayFrame = new ColorSchemeAthleteDisplayFrame();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            //Providate default values if they were not read during deserialization

        }

        [JsonProperty(Order = 1)]
        public ColorSchemeRoot Root { get; set; }


        [JsonProperty(Order = 2)]
        public ColorSchemeResultList ResultList { get; set; }


        [JsonProperty(Order = 3)]
        public ColorSchemeSquaddingList SquaddingList { get; set; }


        [JsonProperty(Order = 4)]
        public ColorSchemeMatchInfoStrip MatchInfoStrip { get; set; }


        [JsonProperty(Order = 5)]
        public ColorSchemeMarquee Marquee { get; set; }


        [JsonProperty(Order = 6)]
        public ColorSchemeImageDisplay ImageDisplay { get; set; }


        [JsonProperty(Order = 7)]
        public ColorSchemeAthleteDisplayFrame AthleteDisplayFrame { get; set; }

    }
}
