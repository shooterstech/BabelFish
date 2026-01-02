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

        public ColorSchemeRoot Root { get; set; }


        public ColorSchemeResultList ResultList { get; set; }


        public ColorSchemeSquaddingList SquaddingList { get; set; }


        public ColorSchemeMatchInfoStrip MatchInfoStrip { get; set; }


        public ColorSchemeMarquee Marquee { get; set; }


        public ColorSchemeImageDisplay ImageDisplay { get; set; }


        public ColorSchemeAthleteDisplayFrame AthleteDisplayFrame { get; set; }

    }
}
